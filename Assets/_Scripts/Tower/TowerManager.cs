using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    [Header("Towers")]
    [SerializeField] private GameObject pistolTower;
    [SerializeField] private GameObject shotgunTower;
    [SerializeField] private GameObject sniperTower;

    [SerializeField] private LayerMask towerLayer;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI towerLevel;
    [SerializeField] private TextMeshProUGUI UpgradeCost;
    [SerializeField] private TextMeshProUGUI towerTargeting;

    private GameObject selectedTower;
    private GameObject placingTower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ClearSelected();
        }

        if (placingTower) {
            if (!placingTower.GetComponent<TowerPlacement>().isPlacing) {
                placingTower = null;
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 100f, towerLayer);

            if (hit.collider != null) {
                if (selectedTower) {
                    GameObject range1 = selectedTower.transform.GetChild(1).gameObject;

                    range1.GetComponent<SpriteRenderer>().enabled = false;
                }

                selectedTower = hit.collider.gameObject;

                GameObject range2 = selectedTower.transform.GetChild(1).gameObject;

                range2.GetComponent<SpriteRenderer>().enabled = true;

                panel.SetActive(true);
                towerName.text = selectedTower.name.Replace("(Clone)", "").Trim();
                towerLevel.text = "Nível: " + selectedTower.GetComponent<TowerUpgrades>().currentlevel.ToString();
                UpgradeCost.text = selectedTower.GetComponent<TowerUpgrades>().currentCost;

                Tower tower = selectedTower.GetComponent<Tower>();
                if (tower.first) {
                    towerTargeting.text = "Modo: Primeiro";
                } else if (tower.last) {
                    towerTargeting.text = "Modo: Último";
                } else if (tower.strong) {
                    towerTargeting.text = "Modo: Forte";
                }
            }
            else if(!EventSystem.current.IsPointerOverGameObject() && selectedTower) {
                panel.SetActive(false);
                GameObject range1 = selectedTower.transform.GetChild(1).gameObject;
                range1.GetComponent<SpriteRenderer>().enabled = false;
                selectedTower = null;
            }
        }
    }

    private void ClearSelected() {
        if (placingTower){
            Destroy(placingTower);
            placingTower = null;
        }
    }

    public void setTower(GameObject tower){
        ClearSelected();
        placingTower = Instantiate(tower);
    }

    public void UpgradeSelected() {
        if (selectedTower) {
            selectedTower.GetComponent<TowerUpgrades>().Upgrade();
            towerLevel.text = "Nível: " + selectedTower.GetComponent<TowerUpgrades>().currentlevel.ToString();
            UpgradeCost.text = selectedTower.GetComponent<TowerUpgrades>().currentCost;
        }
    }

    public void ChangeTargetting(){
        if (selectedTower) {
            Tower tower = selectedTower.GetComponent<Tower>();
            if (tower.first) {
                tower.first = false;
                tower.last = true;
                tower.strong = false;
                towerTargeting.text = "Modo: Último";
            } else if (tower.last) {
                tower.first = false;
                tower.last = false;
                tower.strong = true;
                towerTargeting.text = "Modo: Forte";
            } else if (tower.strong) {
                tower.first = true;
                tower.last = false;
                tower.strong = false;
                towerTargeting.text = "Modo: Primeiro";
            }
        }
    }
}
