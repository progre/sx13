using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressive.Scarlex13Test
{
    class TestTest
    {
        [Test]
        public void AddTest()
        {
            new object().AsDynamic();
            Assert.That(Enumerable.Range(1,3).GetEnumerator().MoveNext(), Is.EqualTo(true));
        }
    }

    internal class Asdf
    {
        private Asdf instance;

        public void hoge()
        {
            
        }
    }

}
