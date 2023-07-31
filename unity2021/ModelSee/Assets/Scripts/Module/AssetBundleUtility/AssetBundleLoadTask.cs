using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity.AssetBundleUtility
{
    public class AssetBundleLoadTask
    {
        public string uri { get; private set; }
        public UnityEvent<AssetBundle> onFinish { get; private set; }

        public AssetBundleLoadTask(string _uri)
        {
            uri = _uri;
            onFinish = new UnityEvent<AssetBundle>();
        }

    }//class
}//namespace
