using UnityEngine;



    public class IKControl : MonoBehaviour
    {

        [SerializeField]
        private bool ikActive = false;
        [SerializeField]
        private Transform rightHandObj = null;
        [SerializeField]
        private Transform leftHandObj = null;
        [SerializeField]
        private Transform rightElbowObj = null;
        [SerializeField]
        private Transform leftElbowObj = null;
        [SerializeField]
        private Transform lookObj = null;
        private GunScript item;
        private Animator animator;
        public bool Holding_gun;

        void Start()
        {
            animator = GetComponent<Animator>();

        }

        void OnAnimatorIK(int layerIndex)
        {

            if (ikActive)
            {

                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }

                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }

                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }

                if (rightElbowObj != null)
                {
                    animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);
                    animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowObj.position);
                    rightElbowObj.LookAt(lookObj);
                }

                if (leftElbowObj != null)
                {
                    animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);
                    animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowObj.position);
                    leftElbowObj.LookAt(lookObj);
                }
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0);
                animator.SetLookAtWeight(0);
            }
        }
        private void Update()
        {
            Hold_Gun();
        }
        public void Hold_Gun()
        {
            item = this.gameObject.GetComponent<GunInventory>().gun_new.GetComponent<GunScript>();
            leftElbowObj = item.LeftElbow;
            rightElbowObj = item.RightElbow;
            rightHandObj = item.RightHand;
            leftHandObj = item.LeftHand;
            item.lookat = lookObj;
        }
    }

