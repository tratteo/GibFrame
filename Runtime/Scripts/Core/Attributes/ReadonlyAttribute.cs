using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GibFrame
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ReadonlyAttribute : PropertyAttribute
    {
    }
}
