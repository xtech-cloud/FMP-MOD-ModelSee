using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using LibMVCS = XTC.FMP.LIB.MVCS;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity.AssetBundleUtility
{
    // Least Recently Used 最近最少使用
    public class CacheStrategyLRU : CacheStrategy
    {
        public CacheStrategyLRU(MonoBehaviour _mono, LibMVCS.Logger _logger ) : base(_mono, _logger)
        {
        }

        public override void Load(string _uri, UnityAction<AssetBundle> _onFinish)
        {
            //TODO Release Pool
            load(_uri, _onFinish);
        }
    }//class
}//namespace
