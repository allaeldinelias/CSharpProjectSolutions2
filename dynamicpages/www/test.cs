using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using WebMatrix.Data;
using System.Configuration;

namespace www
{
    [TestFixture]
    public class test
    {
        [Test]
        public void trivialTest()
        {
            Assert.AreEqual(true, true);
        }
        [Test]
        public void testValidIdentifier()
        {
            Assert.AreEqual(SourceVersion.isName("testMe"), true);
        }
        [Test]
        public void testInValidIdentifier()
        {
            Assert.AreEqual(SourceVersion.isName("test me"), false);
        }
        [Test]
        public void testDatabase()
        {
            try
            {
                var dbMy = Database.Open("MyDatabase");
                Assert.AreEqual(true, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "\n" + e.StackTrace);
                Assert.AreEqual(true, false);
            }
        }
        String sNewInspectionObject = "{\"name\":\"bobsled\", \"archetypeAttributes\":[{\"name\":\"name\", \"SQLType\":\"nvarchar(45)\", \"formType\":\"text\"}, {\"name\":\"dateOfManufacture\", \"SQLType\":\"datetime\", \"formType\":\"date\"}]}";
        [Test]
        public void testNewArchetype()
        {
            try
            {
                Archetype oArchetype = new Archetype(sNewInspectionObject);
                oArchetype.save();
                Assert.AreEqual(true, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "\n" + e.StackTrace);
                Assert.AreEqual(true, false);
            }
        }
        String sNewBobsled ="{\"archetype\":\"bobsled\", \"name\":\"peace train\", \"dateOfManufacture\":\"1997-01-15\"}";
        [Test]
        public void testNewBobsled()
        {
            InspectionObject oBobsled = new InspectionObject(sNewBobsled);
            try
            {
                oBobsled.save();
                Assert.AreEqual(true, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "\n" + e.StackTrace);
                Assert.AreEqual(true, false);
            }
        }
        [Test]
        public void testSQL2JSON()
        {
            string[] aIn = {"bobsled"};
            String sRc = SQL2JSON.toJSON("SELECT * FROM DispAttribute WHERE idDispClass IN (SELECT idDispClass FROM DispClass WHERE name = @col0) ",
            aIn);
            Console.WriteLine(sRc);
            Assert.AreEqual(true, true);
        }
    }
}