using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity
{
    public class GridSpace
    {
        private static GridSpace singleton_ { get; set; }

        public static GridSpace singleton
        {
            get
            {
                return singleton_;
            }
        }

        public static void CreateSingleton()
        {
            singleton_ = new GridSpace();
        }

        public enum Mode
        {
            X,
            Y,
            Z,
            XYZ
        }

        private class Zone
        {
            public Mode mode;
            public Vector3 anchor;
            public int cellSize;
            public List<bool> cellS;
        }

        private Dictionary<string, Zone> zoneS_ = new Dictionary<string, Zone>();


        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="_name">���������</param>
        /// <param name="_mode">�����ģʽ</param>
        /// <param name="_anchor">������������</param>
        /// <param name="_cellSize">����ĵ�Ԫ���С</param>
        public void CreateZone(string _name, Mode _mode, Vector3 _anchor, int _cellSize)
        {
            if (zoneS_.ContainsKey(_name))
                return;

            var zone = new Zone();
            zone.mode = _mode;
            zone.anchor = _anchor;
            zone.cellSize = _cellSize;
            zone.cellS = new List<bool>(); 
            zoneS_[_name] = zone;
        }

        /// <summary>
        /// �½�һ����Ԫ��
        /// </summary>
        /// <param name="_zoneName"></param>
        /// <returns>��Ԫ�����</returns>
        public int NewCell(string _zoneName)
        {
            Zone zone;
            if (!zoneS_.TryGetValue(_zoneName, out zone))
                return -1;

            int index = zone.cellS.Count;
            // �ڻ����б��в��ұ��Ϊδʹ�õĵ�Ԫ��
            for(int i = 0; i < zone.cellS.Count;i++)
            {
                if (!zone.cellS[i])
                {
                    index = i;
                    break;
                }
            }

            // �ѻ���ĵ�Ԫ��ȫ����ʹ��
            if(index == zone.cellS.Count)
            {
                zone.cellS.Add(true);
            }
            zone.cellS[index] = true;
            return index;
        }

        /// <summary>
        /// ɾ��һ����Ԫ��
        /// </summary>
        /// <param name="_zoneName">���������</param>
        /// <param name="_cellIndex">��Ԫ������</param>
        public void DeleteCell(string _zoneName, int _cellIndex)
        {
            Zone zone;
            if (!zoneS_.TryGetValue(_zoneName, out zone))
                return;
            if (_cellIndex >= zone.cellS.Count)
                return;
            // ���Ϊδʹ��
            zone.cellS[_cellIndex] = false;
        }

        /// <summary>
        /// ��ȡ��Ԫ����������
        /// </summary>
        /// <param name="_zoneName"></param>
        /// <param name="_cellIndex"></param>
        /// <returns></returns>
        public Vector3 GetCellCenter(string _zoneName, int _cellIndex)
        {
            Vector3 center = Vector3.zero;
            Zone zone;
            if (!zoneS_.TryGetValue(_zoneName, out zone))
                return center;
            if (_cellIndex >= zone.cellS.Count)
                return center;

            if(zone.mode == Mode.Y)
            {
                center = zone.anchor + new Vector3(0, zone.cellSize * _cellIndex, 0);
            }
            return center;
        }
    }

}//namespace
