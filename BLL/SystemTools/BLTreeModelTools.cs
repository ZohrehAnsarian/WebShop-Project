using BLL.Base;
using Model.UIControls.Tree;
using Model.ToolsModels.Tree;
using System.Collections.Generic;
using System.Linq;
using System;
using Model;
using Model.ViewModels.ProductFeature;
using Model.ViewModels.FeatureType;

namespace BLL.SystemTools
{
    public class BLTreeModelTools : BLBase
    {

        #region int Id

        private bool firstChiled = true;

        public TreeNode GetTreeModel(IEnumerable<ITmNode> treeModelList, string parentId)
        {
            IEnumerable<ITmNode> rawNodeList;

            rawNodeList = treeModelList;

            var rootNode = rawNodeList.FirstOrDefault(r => r.ParentId == -1);
            var children = rawNodeList.Where(c => c.ParentId == rootNode.Id).ToList();
            var jsRootNode = new TreeNode
            {
                id = rootNode.Id.ToString(),
                text = rootNode.Name,
                path = "",
                icon = "",
                a_attr = { href = "#" },
                additionalData = rootNode.AdditionalData,
                LanguageId = rootNode.NodeLanguageId,
                ParentId = parentId,
                Level = 0,
                IsRoot = true,
                IsDefault = rootNode.IsDefault,
            };

            jsRootNode.state.opened = true;

            GenereateTreeModel(jsRootNode, children, 1, rawNodeList);

            return jsRootNode;
        }

        void GenereateTreeModel(TreeNode rootNode, List<ITmNode> children, int level, IEnumerable<ITmNode> rawNodeList)
        {
            if (children.Count == 0)
                return;

            foreach (var node in children)
            {
                var jsNode = new TreeNode
                {
                    id = node.Id.ToString(),
                    text = node.Name,
                    path = (string.IsNullOrWhiteSpace(rootNode.path)) ? node.Name : rootNode.path + "/" + node.Name,
                    icon = "",
                    a_attr = { href = "#" },
                    additionalData = node.AdditionalData,
                    LanguageId = rootNode.LanguageId,
                    ParentId = rootNode.id,
                    IsRoot = false,
                    Level = level,
                    IsDefault = node.IsDefault,
                };

                if (firstChiled)
                {
                    jsNode.state.opened = true;
                    jsNode.state.selected = true;
                    jsNode.a_attr.IsFirstNode = true;

                    firstChiled = false;
                }
                else
                {
                    jsNode.state.opened = false;
                    jsNode.state.selected = false;
                }
                jsNode.state.opened = false;
                jsNode.state.selected = false;

                rootNode.children.Add(jsNode);

                GenereateTreeModel(jsNode, rawNodeList.Where(c => c.ParentId == node.Id).ToList(), level + 1, rawNodeList);

            }

        }
        #endregion int Id

        #region Guid
        private bool firstChiledGuid = true;

        public TreeNode GetTreeModel(IEnumerable<ITmNodeGuid> treeModelList, string parentId)
        {
            IEnumerable<ITmNodeGuid> rawNodeList;

            rawNodeList = treeModelList;

            var rootNode = rawNodeList.FirstOrDefault(r => r.ParentId == Guid.Empty);
            var children = rawNodeList.Where(c => c.ParentId == rootNode.Id).ToList();

            var jsRootNode = new TreeNode
            {
                id = rootNode.Id.ToString(),
                text = rootNode.Name,
                path = rootNode.Name,
                icon = "",
                a_attr = { href = "#" },
                additionalData = rootNode.AdditionalData,
                LanguageId = rootNode.NodeLanguageId,
                ParentId = parentId,
                Level = 0,
                IsRoot = true,
                IsDefault = rootNode.IsDefault,
                description = rootNode.Description,
            };

            jsRootNode.state.opened = true;

            GenereateTreeModel(jsRootNode, children, 1, rawNodeList);

            return jsRootNode;
        }

        void GenereateTreeModel(TreeNode rootNode, List<ITmNodeGuid> children, int level, IEnumerable<ITmNodeGuid> rawNodeList)
        {
            if (children.Count == 0)
                return;

            foreach (var node in children)
            {
                var jsNode = new TreeNode
                {
                    id = node.Id.ToString(),
                    text = node.Name,
                    path = (string.IsNullOrWhiteSpace(rootNode.path)) ? node.Name : rootNode.path + "/" + node.Name,
                    icon = "",
                    a_attr = { href = "#" },
                    additionalData = node.AdditionalData,
                    LanguageId = rootNode.LanguageId,
                    ParentId = rootNode.id,
                    IsRoot = false,
                    Level = level,
                    IsDefault = rootNode.IsDefault,
                    description = rootNode.description,
                };

                if (firstChiled)
                {
                    jsNode.state.opened = true;
                    jsNode.state.selected = true;
                    jsNode.a_attr.IsFirstNode = true;

                    firstChiled = false;
                }
                else
                {
                    jsNode.state.opened = false;
                    jsNode.state.selected = false;
                }

                jsNode.state.opened = false;
                jsNode.state.selected = false;

                rootNode.children.Add(jsNode);

                GenereateTreeModel(jsNode, rawNodeList.Where(c => c.ParentId == node.Id).ToList(), level + 1, rawNodeList);

            }

        }
        #endregion Guid

        #region Common methods

        public List<string> GenerateTreePath(TreeNode tree, string delimiter)
        {
            var paths = ComputePaths(tree, n => n.children);

            var fullPaths = new List<string>();

            foreach (var item in paths)
            {
                var path = string.Empty;

                foreach (var subItem in item)
                {
                    if (string.IsNullOrWhiteSpace(subItem.text) == false)
                    {
                        path += subItem.text + delimiter;
                    }
                }

                if (path.Length > 0)
                {
                    path = path.Substring(0, path.Length - delimiter.Length);
                }

                fullPaths.Add(path);

            }

            return fullPaths;
        }
        public IEnumerable<IEnumerable<T>> ComputePaths<T>(T Root, Func<T, IEnumerable<T>> Children)
        {
            var children = Children(Root);
            if (children != null && children.Any())
            {
                foreach (var Child in children)
                    foreach (var ChildPath in ComputePaths(Child, Children))
                        yield return new[] { Root }.Concat(ChildPath);
            }
            else
            {
                yield return new[] { Root };
            }
        }

        #endregion Common methods

    }
}
