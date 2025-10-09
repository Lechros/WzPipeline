﻿using WzComparerR2.WzLib;

namespace WzComparerR2.Common;

public static class Wz_NodeExtension2
{
    public static Wz_Node GetLinkedSourceNode(this Wz_Node node, GlobalFindNodeFunction findNode, Wz_File wzf = null)
        {
            string path;

            if (!string.IsNullOrEmpty(path = node.Nodes["source"].GetValueEx<string>(null)))
            {
                if (wzf == null) return findNode?.Invoke(path);
                else // Skill 비교 툴팁 출력용 / code from FindNodeByPath
                {
                    string[] fullpath = path.Split('/');
                    Wz_Node returnNode = wzf.Node;
                    foreach (string p in fullpath)
                    {
                        returnNode = returnNode.FindNodeByPath(p, p.Contains(".img"));

                        var img = returnNode.GetValue<Wz_Image>();
                        if (img != null && img.TryExtract()) //判断是否是img
                        {
                            returnNode = img.Node;
                        }
                    }
                    return returnNode;
                }
            }
            else if (!string.IsNullOrEmpty(path = node.Nodes["_inlink"].GetValueEx<string>(null)))
            {
                var img = node.GetNodeWzImage();
                return img?.Node.FindNodeByPath(true, path.Split('/'));
            }
            else if (!string.IsNullOrEmpty(path = node.Nodes["_outlink"].GetValueEx<string>(null)))
            {
                if (wzf == null) return findNode?.Invoke(path);
                else // Skill 비교 툴팁 출력용 / code from FindNodeByPath
                {
                    string[] fullpath = path.Split('/');
                    Wz_Node returnNode = wzf.Node;
                    foreach (string p in fullpath)
                    {
                        returnNode = returnNode.FindNodeByPath(p, p.Contains(".img"));

                        var img = returnNode.GetValue<Wz_Image>();
                        if (img != null && img.TryExtract()) //判断是否是img
                        {
                            returnNode = img.Node;
                        }
                    }
                    return returnNode;
                }
            }
            else
            {
                return node;
            }
        }

        public static Wz_Node HandleFullUol(this Wz_Node node, GlobalFindNodeFunction findNode)
        {
            if (node == null) return null;

            if (node.Value is Wz_Uol uol)
            {
                if (uol.Uol.StartsWith("/"))
                {
                    return findNode?.Invoke(uol.Uol.TrimStart('/'));
                }
                else
                {
                    return uol.HandleUol(node);
                }
            }
            return node;
        }
}