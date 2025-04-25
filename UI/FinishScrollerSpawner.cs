using Riders.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Riders
{
    public class FinishScrollerSpawner : MonoBehaviour
    {
        //Finish Scroller
        [SerializeField] private GameObject finishScroller;
        private List<FinishScroller> queuedScrollers = new List<FinishScroller>();
        [SerializeField] private Transform scrollerSpawnPoint;
        [SerializeField] private float scrollerSpawnRate = 2f;
        static Coroutine scrollerSpawnCycle;


        IEnumerator ScrollerSpawnLoop()
        {
            yield return new WaitForSeconds(scrollerSpawnRate);
            if (queuedScrollers.Count > 0)
            {
                FinishScroller fs = queuedScrollers.ElementAt(0);
                queuedScrollers.Remove(fs);
                fs.gameObject.SetActive(true);
                scrollerSpawnCycle = StartCoroutine(ScrollerSpawnLoop());
            }
        }


        public void SpawnFinishScroller(PlayerController player)
        {
            GameObject g = Instantiate(finishScroller, transform);
            FinishScroller fs = g.GetComponent<FinishScroller>();
            //fs.transform.position = scrollerSpawnPoint.position;

            queuedScrollers.Add(fs);
            TextMeshProUGUI text = fs.GetComponent<TextMeshProUGUI>();
            text.text = player.name + " has finished";
            g.SetActive(false);
            if (scrollerSpawnCycle == null) scrollerSpawnCycle = StartCoroutine(ScrollerSpawnLoop());
        }
    }
}