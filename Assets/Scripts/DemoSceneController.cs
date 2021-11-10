using System.Linq;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    // TODO:
    // Add README.md
    //  explain difference between async and multithreading
    // Add "Open Script" feature
    // Move each example into its own script
    // Add "Using Cancellation Token"
    // Add "https://forum.unity.com/threads/ensurerunningonmainthread-can-only-be-called-from-the-main-thread.396597/#post-2672265"
    // Canno
    public class DemoSceneController : MonoBehaviour
    {
        [SerializeField]
        private CategoryListView categoryListView;

        [SerializeField]
        private CodeExampleListView exampleList;

        [SerializeField]
        private ExampleCategory[] viewModel;

        // private Examplme[] GetCodeExamples()
        // {
        //     return (from guid in AssetDatabase.FindAssets("t:MonoScript")
        //             let path = AssetDatabase.GUIDToAssetPath(guid)
        //             let script = AssetDatabase.LoadAssetAtPath<MonoScript>(path)
        //             let type = script.GetClass()
        //             let att = type.GetCustomAttribute<CodeExampleAttribute>()
        //             where typeof(CodeExample).IsAssignableFrom(scrType)
        //             select (CodeExample)Activator.CreateInstance(scrType))
        //         .ToArray();
        // }


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
                            () => { methodTuple.method.Invoke(null, null); })
                    ).ToArray();

                exampleList.Setup(exampleElementModels);
            };
        }
    }
}