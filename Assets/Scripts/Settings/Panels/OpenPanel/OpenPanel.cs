using UnityEngine;
using UnityEngine.UI;

public abstract class OpenPanel<T> : Panel<T>
where T : BaseObjectData
{
    [SerializeField] ScaleAnimation openScale;
    [SerializeField] ScaleAnimation infoScale;

    [SerializeField] private Text title;
    [SerializeField] private Text cost;

    [SerializeField] private Button openObjectBtn;

    public override void SetInfo(T dataBaseObject)
    {
        title.text = dataBaseObject.Name;
        cost.text = dataBaseObject.OpenCost.Format();

        openObjectBtn.onClick.RemoveAllListeners();

        openObjectBtn.onClick.AddListener
        (
            () =>
            {
                if (dataBaseObject.PaidOpen())
                {
                    openScale.Close();
                    infoScale.Show();
                }
            }
        );
    }

}
