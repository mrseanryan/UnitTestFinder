using Shell.SharePoint.SeismicScreening4D.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Shell.SharePoint.SeismicScreening4D.Framework;

namespace Shell.SharePoint.SeismicScreening4D.BusinessLogic.Test
{
    /// <summary>
    ///This is a test class for QCLogicTest and is intended
    ///to contain all QCLogicTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QCLogicTest
    {
        [TestMethod()]
        public void CalculateQCPropertiesTest()
        {
            FluidSaturations saturations = new FluidSaturations()
            {
                SaturationWater = 1.0,
                SaturationOil = 0.0,
                SaturationGas = 0.0
            };
            FluidProperties initialFluid = new FluidProperties()
            {
                FluidPressure = 22.2,
                FluidTemperature = 60,
                FluidConcentration = 0,
                FluidDensityAPI = 10, //cannot be 0
                GasOilRatioRs = 0,
                FluidGravity = 0.6,
                Mixing = MixingType.Homogeneous,
                Scenario = ProjectScenario.WaterReplacingOil
            };
            //Initial Reservoir Conditions
            RockProperties initialReservoir = new RockProperties()
            {
                DepthBelowMudline = 1524,
                //TrendType = TrendTypes.BongaTrend,
                ReservoirType = ReservoirTypes.Laminated,
                VpSand = 2415,
                VsSand = 1040,
				VpSandClean = 2415,
				VsSandClean = 1040,
				PorositySand = 0.3,
                VpShale = 2370,
                VsShale = 994,
                RhoShale = 2258,
                FractionShale = 0.4,
                KGrain = 37,
                MuGrain = 44,
                RhoGrain = 2650,
                FluidSaturationsInRock = saturations
            };
            PreferredUnitSystem unitsystem = new MetricMPa();
            QCProperties expected = new QCProperties()
            {
                Porosity = 0.3, //0.34068825,
                KDryByKMin = 0.107294,
                DenBrineSat = 2121.933,
                VpBrineSat = 2415.363,
                VsBrineSat = 1040.438
            };
            QCProperties actual;
            actual = ScenarioResults.GetQCResults(initialFluid, initialReservoir, new MetricMPa());
            //To test in Field units actual = QCLogic.CalculateQCProperties(rock, fluid, flagResults, new Field()) and convert values for assertion.
            Assert.AreEqual(expected.Porosity, actual.Porosity, "Porosity do not match");
            //Assert.AreEqual(expected.KDryByKMin, actual.KDryByKMin, 1e-3, "KDryByKmin do not match");
            //Assert.AreEqual(expected.DenBrineSat.Value, actual.DenBrineSat.Value, 1e-3, "DenBrineSat do not match");
            //Assert.AreEqual(expected.VpBrineSat.Value, actual.VpBrineSat.Value, 1e-3, "VpBrineSat do not match");
            //Assert.AreEqual(expected.VsBrineSat.Value, actual.VsBrineSat.Value, 1e-3, "VpBrineSat do not match");
            PorosityKDryByKMinProperties kdryValueForPointZero2 = new PorosityKDryByKMinProperties()
            {
                porosity = 0.02,
                KDryByKMPoint1 = 0.833333333,
                KDryByKMPoint15 = 0.882352941,
                KDryByKMPoint2 = 0.909090909,
                KDryByKMPoint25 = 0.925925926,
                KDryByKMPoint3 = 0.9375,
                KDryByKMPoint35 = 0.945945946,
                KDryByKMPoint4 = 0.952380952,
                KDryByKMPoint45 = 0.957446809,
                KDryByKMPoint5 = 0.961538462
            };
            PorosityKDryByKMinProperties kdryValueForPoint98 = new PorosityKDryByKMinProperties()
            {
                KDryByKMPoint1 = 0.0925925925925926,
                KDryByKMPoint15 = 0.13274336283185842,
                KDryByKMPoint2 = 0.16949152542372883,
                KDryByKMPoint25 = 0.2032520325203252,
                KDryByKMPoint3 = 0.234375,
                KDryByKMPoint35 = 0.26315789473684209,
                KDryByKMPoint4 = 0.28985507246376813,
                KDryByKMPoint45 = 0.31468531468531469,
                KDryByKMPoint5 = 0.33783783783783783,
                porosity = 0.98
            };
            VpVsProperties vp1700 = new VpVsProperties()
            {
                CastagniaDolomite = 913.74,
                CastagniaLimestone = 538.8788,
                CastagniaMudrock = 293.17,
                CastagniaSandstone = 511.24,
                Han = 562.32,
                Vp = 1700
            };
            DenVpProperties denVp1700 = new DenVpProperties()
            {
                GardnerAnhydrite = 2384.05319906545,
                GardnerDolomite = 1988.94318029729,
                GardnerSandstone = 1906.5809814135,
                GardnerShale = 2014.22050513369,
                MavkoLimestone = 1381.579861,
                Vp = 1700
            };
            //Assert.AreEqual(kdryValueForPoint98, actual.PorosityKDryPlotList[1], "Kdry table does not match when porosity is 0.98", null);
            //Assert.AreEqual(vp1700, actual.VpVsPlotList[1], "VpVs table does not match when Vp is 1700", null);
            //Assert.AreEqual(denVp1700, actual.DenVpPlotList[1], "DenVp table does not match when Vp is 1700", null);
        }

        /// <summary>
        ///A test for CalculateQCProperties
        ///</summary>
        [TestMethod()]
        public void CalculateQCPropertiesOnlyTest()
        {
            RockProperties rock = new RockProperties()
            {
                RockPorosity = 0.34068825,
                KGrain = 37,
                MuGrain = 44,
                RhoGrain = 2650,
                RhoBulk = 2121.933213,
                KBulk = 9.316624568,
                MuBulk = 2.297016025
            };
            FluidProperties fluid = new FluidProperties()
            {
                GassmanResultKInitial = 2.54,
                GassmanResultRhoInitial = 1100
            };
            FluidProperties[] flagResults = new FluidProperties[] 
            { 
                new FluidProperties(){FluidType = FluidType.Brine, FlagResultK = 2.54, FlagResultRho = 1100}
            };
            QCProperties expected = new QCProperties()
            {
                Porosity = 0.34068825,
                KDryByKMin = 0.107294,
                DenBrineSat = 2121.933,
                VpBrineSat = 2415.363,
                VsBrineSat = 1040.438
            };
            QCProperties actual;
            actual = QCLogic.CalculateQCProperties(rock, fluid, flagResults, new MetricMPa());
            //To test in Field units actual = QCLogic.CalculateQCProperties(rock, fluid, flagResults, new Field()) and convert values for assertion.
            Assert.AreEqual(expected.Porosity, actual.Porosity, "Porosity do not match");
            Assert.AreEqual(expected.KDryByKMin, actual.KDryByKMin, 1e-3, "KDryByKmin do not match");
            Assert.AreEqual(expected.DenBrineSat.Value, actual.DenBrineSat.Value, 1e-3, "DenBrineSat do not match");
            Assert.AreEqual(expected.VpBrineSat.Value, actual.VpBrineSat.Value, 1e-3, "VpBrineSat do not match");
            Assert.AreEqual(expected.VsBrineSat.Value, actual.VsBrineSat.Value, 1e-3, "VpBrineSat do not match");
            PorosityKDryByKMinProperties kdryValueForPointZero2 = new PorosityKDryByKMinProperties()
            {
                porosity = 0.02,
                KDryByKMPoint1 = 0.833333333,
                KDryByKMPoint15 = 0.882352941,
                KDryByKMPoint2 = 0.909090909,
                KDryByKMPoint25 = 0.925925926,
                KDryByKMPoint3 = 0.9375,
                KDryByKMPoint35 = 0.945945946,
                KDryByKMPoint4 = 0.952380952,
                KDryByKMPoint45 = 0.957446809,
                KDryByKMPoint5 = 0.961538462
            };
            PorosityKDryByKMinProperties kdryValueForPoint98 = new PorosityKDryByKMinProperties()
            {
                KDryByKMPoint1= 0.0925925925925926,
                KDryByKMPoint15= 0.13274336283185842,
                KDryByKMPoint2= 0.16949152542372883,
                KDryByKMPoint25= 0.2032520325203252,
                KDryByKMPoint3=0.234375,
                KDryByKMPoint35= 0.26315789473684209,
                KDryByKMPoint4=0.28985507246376813,
                KDryByKMPoint45= 0.31468531468531469,
                KDryByKMPoint5=0.33783783783783783,
                porosity= 0.98
            };
            VpVsProperties vp1700 = new VpVsProperties()
            {
                CastagniaDolomite = 913.74,
                CastagniaLimestone = 538.8788,
                CastagniaMudrock = 293.17,
                CastagniaSandstone = 511.24,
                Han = 562.32,
                Vp = 1700
            };
            DenVpProperties denVp1700 = new DenVpProperties()
            {
                GardnerAnhydrite= 2384.05319906545,
                GardnerDolomite=1988.94318029729,
                GardnerSandstone= 1906.5809814135,
                GardnerShale=2014.22050513369,
                MavkoLimestone= 1381.579861,
                Vp=1700
            };
            //Assert.AreEqual(kdryValueForPoint98, actual.PorosityKDryPlotList[1], "Kdry table does not match when porosity is 0.98", null);
            //Assert.AreEqual(vp1700, actual.VpVsPlotList[1], "VpVs table does not match when Vp is 1700", null);
            //Assert.AreEqual(denVp1700, actual.DenVpPlotList[1], "DenVp table does not match when Vp is 1700", null);
        }

        /// <summary>
        ///A test for CalculatePorosityKDryByKMinProperties
        ///</summary>
        [TestMethod()]
        public void CalculatePorosityKDryByKMinPropertiesTest()
        {

            //List<PorosityKDryByKMinProperties> expected = new List<PorosityKDryByKMinProperties>(); 
            //PorosityKDryByKMinProperties kdryValueForPointZero2 = new PorosityKDryByKMinProperties()
            //{
            //        porosity = 0.02,
            //        KDryByKMPoint1 = 0.833333333 , 
            //        KDryByKMPoint15 = 0.882352941,
            //        KDryByKMPoint2 = 0.909090909,
            //        KDryByKMPoint25 = 0.925925926,
            //        KDryByKMPoint3 = 0.9375,
            //        KDryByKMPoint35 = 0.945945946,
            //        KDryByKMPoint4 = 0.952380952,
            //        KDryByKMPoint45 = 0.957446809,
            //        KDryByKMPoint5 = 0.961538462
                
            //}
            //List<PorosityKDryByKMinProperties> actual;
            //var unitMgr = new Seismic4DUnitManager();
            //var metric = new MetricMPa();
            //actual = QCLogic.CalculateQCProperties();
            //Assert.AreEqual(expected, actual, 1e-4, "Kphi/Km vs Kdry/Km series don't match");
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CalculateVpVsProperties
        ///</summary>
        [TestMethod()]
        public void CalculateVpVsPropertiesTest()
        {
            //Visually test using the debug table in the UI.

            //List<VpVsProperties> expected = null; // TODO: Initialize to an appropriate value
            //List<VpVsProperties> actual;
            //actual = QCLogic.CalculateVpVsProperties();
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CalculateDenVpProperties
        ///</summary>
        [TestMethod()]
        public void CalculateDenVpPropertiesTest()
        {
            //Visually test using the debug table in the UI.

            //List<DenVpProperties> expected = null; // TODO: Initialize to an appropriate value
            //List<DenVpProperties> actual;
            //actual = QCLogic.CalculateDenVpProperties();
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
