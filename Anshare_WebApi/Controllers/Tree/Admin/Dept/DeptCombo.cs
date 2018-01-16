using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Model;
namespace api.Controllers
{
    public class DeptCombo
    {
        //public static Guid GID;
        public static string GetJson() ///////////////这个ID是PersonInfo中的ID
        {
            //GID = ID;
            string json = "[";
            IList<Tree> t = DeptComboDB.returnParentTreeModel();
            foreach (Tree model in t)
            {
                if (model != t[t.Count - 1])
                {
                    json += GetJsonByModel(model) + ",";
                }
                else
                {
                    json += GetJsonByModel(model);
                }
            }
            json += "]";
            Debug.WriteLine(json);
            //json = json.Replace("#", "'");
            return json;
        }

        public static string GetJsonByModel(Tree t)
        {
            string json = "";
            bool flag = DeptComboDB.isHaveChild(t.id);/////////////////////////////////////////////////id为父节点的ID

            json = "{"
                      + "\"id\":\"" + t.id + "\","
                      + "\"text\":\"" + t.text + "\","
                      + "\"iconCls\":\"ok\",";
            if (t.@checked != null)
            {
                json += "\"checked\":true,";
            }
            json += "\"children\":";
            if (!flag)
            {
                json += "null}";
            }
            else
            {
                json += "[";
                IList<Tree> list = DeptComboDB.getChild(t.id);
                // IList<Tree> list = UnitComboTreeDB.getChild(GID, t.id);
                foreach (Tree atom in list)
                {
                    if (atom != list[list.Count - 1])
                    {
                        json += GetJsonByModel(atom) + ",";
                    }
                    else
                    {
                        json += GetJsonByModel(atom);
                    }
                }
                json += "]}";
            }
            return json;
        }

    }
}