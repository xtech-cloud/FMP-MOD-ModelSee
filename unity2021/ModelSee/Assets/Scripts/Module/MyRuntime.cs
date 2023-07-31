
using System.Collections.Generic;
using UnityEngine;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.ModelSee.LIB.MVCS;
using XTC.FMP.MOD.ModelSee.LIB.Unity.AssetBundleUtility;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity
{
    /// <summary>
    /// 运行时类
    /// </summary>
    ///<remarks>
    /// 存储模块运行时创建的对象
    ///</remarks>
    public class MyRuntime : MyRuntimeBase
    {
        public MyRuntime(MonoBehaviour _mono, MyConfig _config, MyCatalog _catalog, Dictionary<string, LibMVCS.Any> _settings, LibMVCS.Logger _logger, MyEntryBase _entry)
            : base(_mono, _config, _catalog, _settings, _logger, _entry)
        {
            GridSpace.CreateSingleton();
            AssetBundleLoader.CreateSingleton(new CacheStrategyLRU(_mono, _logger));
        }
    }
}

