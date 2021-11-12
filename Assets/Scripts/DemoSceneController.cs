using System.Linq;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class DemoSceneController : MonoBehaviour
    {
        [SerializeField]
        private CategoryListView categoryListView;

        [SerializeField]
        private CodeExampleListView exampleList;

        [SerializeField]
        private ExampleCategory[] viewModel;
        
        private void Awake()
        {
            InitView();
        }

        private void InitView()
        {
            categoryListView.Init(viewModel);
            categoryListView.SelectionChanged += category =>
            {
                var exampleElementModels = (from script in category.scripts
                        from methodTuple in ExampleScriptUtils.GetAllExampleMethods(script)
                        select new ExampleElementModel(methodTuple.att.Title,
                            script,
                            () =>
                            {
                                ThreadLogger.ClearColorCache();
                                methodTuple.method.Invoke(null, null);
                            })
                    ).ToArray();

                exampleList.Setup(exampleElementModels);
            };
        }
    }
}