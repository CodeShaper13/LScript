using LScript;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptPreview : MonoBehaviour {

    [SerializeField, Required]
    private GameObject _prefabBrick = null;
    [SerializeField, Required]
    private BrickColorList _colorList = null;

    private List<GameObject> objs = new List<GameObject>();

    public void setBricks(List<BrickColor> bricks) {
        this.clear();

        foreach(BrickColor brick in bricks) {
            GameObject obj = GameObject.Instantiate(this._prefabBrick, this.transform);
            obj.GetComponent<Image>().color = this._colorList.brickToRgb(brick);

            this.objs.Add(obj);
        }
    }

    public void clear() {
        if(this.objs != null) {
            foreach(GameObject obj in this.objs) {
                GameObject.Destroy(obj);
            }

            this.objs.Clear();
        }
    }
}