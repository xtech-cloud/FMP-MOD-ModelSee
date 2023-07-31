using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity.AssetBundleUtility
{
    public class AssetBundleLoader
    {
        public static AssetBundleLoader instance { get; private set; }
        private CacheStrategy cacheStrategy_ = null;

        public static void CreateSingleton(CacheStrategy _strategy)
        {
            instance = new AssetBundleLoader();
            instance.cacheStrategy_ = _strategy;
        }

        public void Load(string _uri, UnityAction<AssetBundle> _onFinish)
        {
            cacheStrategy_.Load(_uri, _onFinish);
        }
    }//class
}//namespace
