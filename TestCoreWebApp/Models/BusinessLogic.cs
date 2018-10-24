using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoreWebApp.Models
{
    public class BusinessLogic
    {
        public BusinessLogic()
        {
        }

        public void DoTest()
        {
            //throw a dummy null reference exception
            int[] array = null;
            try
            {
                int i = array.Length;
            }
            catch (Exception e)
            {
                LogEx.ExceptionManager.Instance.CatchException(e);
            }
        }
    }
}
