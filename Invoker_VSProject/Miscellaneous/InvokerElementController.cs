using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using EntityStates;

namespace Invoker.Miscellaneous
{
    class InvokerElementController : MonoBehaviour
    {
        public List<InvokerElement> currentElements;

        private CharacterBody characterBody;
        private CharacterMotor characterMotor;
        private ChildLocator childLocator;
        private Transform rootTransform;
        private Animator animator;
        private Vector3 velocity1 = Vector3.zero;
        private Vector3 velocity2 = Vector3.zero;
        private Vector3 centerPosition;
        private float radius = 0.6f;
        private float rotationSpeed = 2.5f;

        public void Start()
        {
            this.characterBody = base.GetComponent<CharacterBody>();
            this.characterMotor = this.characterBody.characterMotor;
            this.childLocator = this.characterBody.modelLocator.modelTransform.GetComponent<ChildLocator>();
            this.rootTransform = this.childLocator.FindChild("Root");
            this.animator = this.characterBody.modelLocator.modelTransform.GetComponent<Animator>();
            this.centerPosition = this.characterBody.transform.position;
            this.currentElements = new List<InvokerElement>();

            this.currentElements.Add(new InvokerElement(ElementType.Quas));
            this.currentElements.Add(new InvokerElement(ElementType.Wex));
            this.currentElements.Add(new InvokerElement(ElementType.Exort));
            this.UpdateElementBuffs();
        }

        public void Update()
        {
            for(int i = 0; i < this.currentElements.Count; i++)
            {
                if (!this.currentElements[i].gameObject) continue;

                //This disables the elements whenever charactermotor is disabled; basically is just a fix for orbs existing before Invoker comes out of drop pod.
                //Unsure if it will cause problems.
                if (!characterMotor.enabled)
                {
                    this.currentElements[i].gameObject.SetActive(false);
                    this.centerPosition = this.rootTransform.position;
                    continue;
                }

                this.currentElements[i].gameObject.SetActive(true);
                this.centerPosition = this.rootTransform.position + Vector3.up * 1.5f;
                float theta = (2*Mathf.PI / this.currentElements.Count) * i + Time.time * this.rotationSpeed;
                float y = this.centerPosition.y + Mathf.Sin(Time.time + i)/3.5f;
                float x = this.centerPosition.x + radius * Mathf.Cos(theta);
                float z = this.centerPosition.z + radius * Mathf.Sin(theta);

                this.currentElements[i].gameObject.transform.position = Vector3.SmoothDamp(this.currentElements[i].gameObject.transform.position, new Vector3(x, y, z), ref this.velocity2, 0.04f);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchElement(ElementType.Quas, this.PlayAnimation());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchElement(ElementType.Wex, this.PlayAnimation());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchElement(ElementType.Exort, this.PlayAnimation());
            }
        }

        //Replace the oldest element with a new one of elementType with initial position at the position of child with childName.
        //Note: deletes the oldest element.
        //This whole system does not have a hardcoded cap at 3 elements so theoretically you could make an AddElement() function which does not delete the last element if that was desired.
        public void SwitchElement(ElementType elementType, string childName = "")
        {
            int count = this.currentElements.Count;

            this.currentElements[count-1].DestroyElement();

            for(int i = count-1; i >= 1 ; i--)
            {
                this.currentElements[i] = this.currentElements[i-1];
            }

            this.currentElements[0] = new InvokerElement(elementType);
            if(childName != "")
            {
                this.currentElements[0].gameObject.transform.position = this.childLocator.FindChild(childName).position;
            }
            else
            {
                this.currentElements[0].gameObject.transform.position = this.rootTransform.position;
            }

            Util.PlaySound(Core.Assets.elementSwitchSound, base.gameObject);
            this.UpdateElementBuffs();
        }

        //Plays a random one of Invoker's orb spawn animations and returns the childname of the hand involved.
        private string PlayAnimation()
        {
            bool random = (UnityEngine.Random.value > 0.5f);

            if (random)
            {
                EntityState.PlayAnimationOnAnimator(this.animator, "Gesture, Override", "Orb1");
                return "HandL";
            }
            else
            {
                EntityState.PlayAnimationOnAnimator(this.animator, "Gesture, Override", "Orb2");
                return "HandR";
            }
        }

        private void UpdateElementBuffs()
        {

            int quasCount = 0;
            int wexCount = 0;
            int exortCount = 0;

            foreach(InvokerElement i in this.currentElements)
            {
                switch (i.elementType)
                {
                    case ElementType.Quas:
                        quasCount++;
                        break;
                    case ElementType.Wex:
                        wexCount++;
                        break;
                    case ElementType.Exort:
                        exortCount++;
                        break;
                }
            }

            this.characterBody.SetBuffCount(InvokerPlugin.quasBuff, quasCount);
            this.characterBody.SetBuffCount(InvokerPlugin.wexBuff, wexCount);
            this.characterBody.SetBuffCount(InvokerPlugin.exortBuff, exortCount);
        }
    }

    struct InvokerElement
    {
        public GameObject gameObject;
        public ElementType elementType;

       public InvokerElement(ElementType elementType)
        {
            this.elementType = elementType;
            this.gameObject = null;

            switch (this.elementType)
            {
                case ElementType.Quas:
                    this.gameObject = UnityEngine.Object.Instantiate(Core.Assets.quasElementPrefab);
                    break;
                case ElementType.Wex:
                    this.gameObject = UnityEngine.Object.Instantiate(Core.Assets.wexElementPrefab);
                    break;
                case ElementType.Exort:
                    this.gameObject = UnityEngine.Object.Instantiate(Core.Assets.exortElementPrefab);
                    break;
            } 
        }

        public void DestroyElement()
        {
            if (this.gameObject) UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    enum ElementType
    {
        Quas,
        Wex,
        Exort
    }
}
