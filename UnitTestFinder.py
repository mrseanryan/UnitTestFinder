"""
UnitTestFinder.py

Finds the unit tests in a sourcecode base, and outputs a summary to a text file.

USAGE: UnitTestFinder.py <directory to scan> <output text file>
"""

import glob # glob supports Unix style pathname extensions
import os
import pdb
import sys

def showUsage():
	print "USAGE: UnitTestFinder.py <directory to scan> <output text file>"

##################################################
#constants
ENDLINE = "\r\n" #Windows

##################################################
#class FileReader
class FileReader :

	def __init__(self,filepath):
		self.file = open(filepath)
		self.eof = False

	def GetNextLine(self):
		try:
			return self.file.next()
		except StopIteration: 
			self.eof = True
		pass
		

	def IsEOF(self):
		return self.eof

##################################################
#parsers

#TODO - make a base class, to reuse parsing code

##################################################
#parsers - C#
class ParserCS :
	def __init__(self):
		self.lang = "C#"
		self.newLine = ENDLINE
	
	def getExtensions(self):
		extns = []
		extns.append('cs')
		
		return extns

	def getLang(self):
		return self.lang
	
	def parseClassNameFromLine(self, nextLine):
		nextLine = nextLine.replace("public class", "")
		nextLine = nextLine.strip()
		return nextLine
	
	def getClassStartTokens(self):
		return [ "[TestClass]", "[TestClass()]" ]
	
	def checkForClassStart(self, line, reader):
		for token in self.getClassStartTokens():
			if token in line:
				nextLine = reader.GetNextLine()
				className = self.parseClassNameFromLine(nextLine)
				return className
		return ""
	
	def parseMethodNameFromLine(self, line):
		line = line.replace("public void", "")
		line = line.replace("()", "")
		line = line.strip()
		return line

	def getTestStartTokens(self):
		tokens = ["[TestMethod]", "[TestMethod()]"]
		return tokens
		
	def checkForTestStart(self, line, reader):
		for tok in self.getTestStartTokens():
			if tok in line:
				nextLine = reader.GetNextLine()
				className = self.parseMethodNameFromLine(nextLine)
				return className
		return ""
		
	def createSummary(self, newTest, currentClass):
		return "lang: " + self.getLang() + " class: " + currentClass + " test: " + newTest + self.newLine
	
	def GetDescription(self):
		return "Unit Tests in " + self.getLang()
	
	def parseFile(self, filePath):
		print('parsing file ' + filePath + '...')
		
		#pdb.set_trace()
		
		reader = FileReader(filePath)
		
		testSummaries = []
		
		fileHeader = "file: " + filePath + ENDLINE
		
		currentClass = "(unknown)"

		line = reader.GetNextLine()
		while(not reader.IsEOF()):
			nextClass = self.checkForClassStart(line, reader)
			if(len(nextClass)>0):
				currentClass = nextClass
			newTest = self.checkForTestStart(line, reader)
			if(len(newTest) > 0):
				testSummaries.append(self.createSummary(newTest, currentClass))
			line = reader.GetNextLine()
		
		result = (fileHeader, testSummaries)
		return result

##################################################

#factory method, for the parsers
def createParsers():
	parsers = []
	parser = ParserCS()
	
	parsers.append(parser)

	return parsers


def processDir(dirpathToScan, outputTextFilepath, summaryFile):
	
	parsers = createParsers()
	
	for par in parsers:
		print('parsing ' + par.getLang() + '...')
		
		countOfSummaries = 0
		
		#summaryFile.write(par.GetDescription() + ENDLINE)
		#summaryFile.write("=" * len(par.GetDescription()) + ENDLINE)
		
		subdirList = []
		
		for ext in par.getExtensions():
		
			#find the files:
			print('scanning for files: *.' + ext)
			
			for filename in os.listdir(dirpathToScan):
				filePath = os.path.join(dirpathToScan, filename)
				if os.path.isfile(filePath):
					if filename.lower().endswith("." + ext):
						print '    ------> ' + filePath
						
						(fileHeader, summariesThisFile) = par.parseFile(filePath)
						if(len(summariesThisFile) > 0):
							countOfSummaries = countOfSummaries + len(summariesThisFile)
							summaryFile.write(fileHeader)
							for summary in summariesThisFile:
								summaryFile.write(summary)
							summaryFile.write(ENDLINE)
				else:
					subdirList.append(filePath)

		for subdir in subdirList:
			countOfSummaries = countOfSummaries + processDir(subdir, outputTextFilepath, summaryFile)

		return countOfSummaries

##################################################
#main
def main(argv):
	if len(argv) != 3:
		showUsage()
		return 1
	dirpathToScan = argv[1]
	outputTextFilepath = argv[2]
	
	summaryFile = open(outputTextFilepath, "w+")
	countOfSummaries = processDir(dirpathToScan, outputTextFilepath, summaryFile)
	overallSummary = str(countOfSummaries) + " tests found."
	summaryFile.write(overallSummary)
	print(overallSummary)
	
if __name__ == "__main__":
	main(sys.argv)

