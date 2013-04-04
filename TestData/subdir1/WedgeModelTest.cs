using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shell.SharePoint.SeismicScreening4D.BusinessLogic;
using Shell.SharePoint.SeismicScreening4D.Framework;

namespace Shell.SharePoint.SeismicScreening4D.BusinessLogic.Test
{
    [TestClass]
    public class WedgeModelTest
    {
        /// <summary>
        /// Linspaces the python test.
        /// </summary>
        [TestMethod]
        public void LinspacePythonTest()
        {
            List<double> results = ListManipulationLogic.Linspace(2, 4, 0.5, false);
        }

        //TODO - is this test no longer valid ?  review this test against spec
        //
        //[TestMethod]
        //public void ConvolveTest()
        //{
        //    List<double> signal = new List<double>(){ 3.0, 4.0, 5.0 };
        //    List<double> filter = new List<double> { 2.0, 1.0};
        //    double[] results = WedgeModel.Convolve(signal, filter);
        //    //Test 1 - 6,11,14,5
        //}
        /// <summary>
        /// Convolves the test.
        /// </summary>
        [TestMethod]
        public void ConvolveTest()
        {
            List<double> signal = new List<double>() { 1, 1, 1 };
            List<double> filter = new List<double> { 1,2,3 };
            List<double> results = ListManipulationLogic.Convolve(signal, filter);
        }

        //TODO - is this test no longer valid ? review this test against spec
        //
        ///// <summary>
        ///// Calculates the trace test.
        ///// </summary>
        //[TestMethod]
        //public void CalculatetraceTest()
        //{
        //    WedgeModel wedge = new WedgeModel()
        //    {
        //        DtSampleRateOfOutput = 1e-3,
        //        modelVp = { 2000, 3600, 2000 },
        //        modelRho = { 1800, 2200, 1800 },
        //        ModelThickness = ListManipulationLogic.Zeros(3, 20)
        //    };
        //    wedge.Calculatetraces(wedge);
        //    ArrayList results = wedge.traces;
        //}
    }
}
