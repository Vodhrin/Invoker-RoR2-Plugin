using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;

namespace Invoker.Miscellaneous
{
    class InvokerElementController : MonoBehaviour
    {
        public List<InvokerElement> currentElements;

        private CharacterBody characterBody;
        private ChildLocator childLocator;
        private Vector3 velocity = Vector3.zero;
        private Vector3 centerPosition;
        private float radius = 1;
        private float rotationSpeed = 3;

        public void Start()
        {
            this.characterBody = base.GetComponent<CharacterBody>();
            this.childLocator = characterBody.modelLocator.modelTransform.GetComponent<ChildLocator>();
            this.centerPosition = this.characterBody.transform.position;
            this.currentElements = new List<InvokerElement>();

            this.currentElements.Add(new InvokerElement(OrbType.Quas));
            this.currentElements.Add(new InvokerElement(OrbType.Wex));
            this.currentElements.Add(new InvokerElement(OrbType.Exort));
        }

        public void Update()
        {
            for(int i = 0; i < this.currentElements.Count; i++)
            {
                if (!this.currentElements[i].gameObject) continue;

                if (this.currentElements[i].gameObject)
                {
                    Transform rootTransform = this.childLocator.FindChild("Root");
                    this.centerPosition = Vector3.SmoothDamp(this.centerPosition, rootTransform.position + Vector3.up * 1.5f, ref this.velocity, 0.2f);
                    float theta = (2*Mathf.PI / this.currentElements.Count) * i + Time.time * this.rotationSpeed;
                    float y = this.centerPosition.y;
                    float x = this.centerPosition.x + radius * Mathf.Cos(theta);
                    float z = this.centerPosition.z + radius * Mathf.Sin(theta);

                    this.currentElements[i].gameObject.transform.position = new Vector3(x, y, z);
                }
            }
        }
    }

    struct InvokerElement
    {
        public GameObject gameObject;
        public OrbType orbType;

       public InvokerElement(OrbType orbType)
        {
            this.orbType = orbType;
            this.gameObject = null;

            switch (this.orbType)
            {
                case OrbType.Quas:
                    this.gameObject = UnityEngine.Object.Instantiate(Core.Assets.quasElementPrefab);
                    break;
                case OrbType.Wex:
                    this.gameObject = UnityEngine.Object.Instantiate(Core.Assets.wexElementPrefab);
                    break;
                case OrbType.Exort:
                    this.gameObject = UnityEngine.Object.Instantiate(Core.Assets.exortElementPrefab);
                    break;
            } 
        }

        public void DestroyElement()
        {
            if (this.gameObject) UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    enum OrbType
    {
        Quas,
        Wex,
        Exort
    }
}
