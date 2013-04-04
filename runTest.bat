IF NOT EXIST temp (MKDIR temp)

UnitTestFinder.py TestData temp\summary.txt 

type temp\summary.txt

ECHO There should be 13 tests found.
