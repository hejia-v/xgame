namespace KBEngine
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Avatar : KBEngine.KBEGameObject
    {
        public Combat combat = null;

        public static SkillBox skillbox = new SkillBox();

        public Avatar()
        {
        }

        public override void __init__()
        {
            combat = new Combat(this);

            if (isPlayer())
            {
                Event.registerIn("relive", this, "relive");
                Event.registerIn("useTargetSkill", this, "useTargetSkill");
                Event.registerIn("jump", this, "jump");
                Event.registerIn("updatePlayer", this, "updatePlayer");
            }
        }

        public override void onDestroy()
        {
            if (isPlayer())
            {
                KBEngine.Event.deregisterIn(this);
            }
        }

        public void relive(Byte type)
        {
            cellCall("relive", type);
        }

        public bool useTargetSkill(Int32 skillID, Int32 targetID)
        {
            Skill skill = SkillBox.inst.get(skillID);
            if (skill == null)
                return false;

            SCEntityObject scobject = new SCEntityObject(targetID);
            if (skill.validCast(this, scobject))
            {
                skill.use(this, scobject);
                return true;
            }

            Dbg.DEBUG_MSG(className + "::useTargetSkill: skillID=" + skillID + ", targetID=" + targetID + ". is failed!");
            return false;
        }

        public void jump()
        {
            cellCall("jump");
        }

        public virtual void onJump()
        {
            Dbg.DEBUG_MSG(className + "::onJump: " + id);
            Event.fireOut("otherAvatarOnJump", new object[] { this });
        }

        public virtual void updatePlayer(UInt32 currSpaceID, float x, float y, float z, float yaw)
        {
            // 更加安全的更新位置，避免将上一个场景的坐标更新到当前场景中的玩家
            if (currSpaceID > 0 && currSpaceID != KBEngineApp.app.spaceID)
            {
                return;
            }

            position.x = x;
            position.y = y;
            position.z = z;

            direction.z = yaw;
        }

        public virtual void onAddSkill(Int32 skillID)
        {
            Dbg.DEBUG_MSG(className + "::onAddSkill(" + skillID + ")");
            Event.fireOut("onAddSkill", new object[] { this });

            Skill skill = new Skill();
            skill.id = skillID;
            skill.name = skillID + " ";
            switch (skillID)
            {
                case 1:
                    break;
                case 1000101:
                    skill.canUseDistMax = 20f;
                    break;
                case 2000101:
                    skill.canUseDistMax = 20f;
                    break;
                case 3000101:
                    skill.canUseDistMax = 20f;
                    break;
                case 4000101:
                    skill.canUseDistMax = 20f;
                    break;
                case 5000101:
                    skill.canUseDistMax = 20f;
                    break;
                case 6000101:
                    skill.canUseDistMax = 20f;
                    break;
                default:
                    break;
            };

            SkillBox.inst.add(skill);
        }

        public virtual void onRemoveSkill(Int32 skillID)
        {
            Dbg.DEBUG_MSG(className + "::onRemoveSkill(" + skillID + ")");
            Event.fireOut("onRemoveSkill", new object[] { this });
            SkillBox.inst.remove(skillID);
        }

        public override void onEnterWorld()
        {
            base.onEnterWorld();

            if (isPlayer())
            {
                Event.fireOut("onAvatarEnterWorld", new object[] { KBEngineApp.app.entity_uuid, id, this });
                SkillBox.inst.pull();
            }
        }
    }
}
