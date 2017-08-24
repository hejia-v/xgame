namespace KBEngine
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class KBEGameObject : if_Entity_error_use______git_submodule_update_____kbengine_plugins_______open_this_file_and_I_will_tell_you
    {
        public KBEGameObject()
        {
        }

        /*
            以下函数是实体的属性被设置时插件底层调用
            set_属性名称(), 想监听哪个属性就实现该函数，事件触发后由于world.cs中监听了该事件，world.cs会取出数据做行为表现。
            另外，这些属性如果需要同步到客户端，前提是在def中定义过该属性，并且属性的广播标志为ALL_CLIENTS、OTHER_CLIENTS、等等，
            参考：http://www.kbengine.org/cn/docs/programming/entitydef.html
            
            实际下列函数可以再抽象出一些层次 
            例如Combat.cs对应服务端demo中的kbengine_demos_assets\scripts\cell\interfaces\Combat.py|CombatPropertys.py, 
            像HP、MP、recvDamage都属于战斗相关
            
            set_state可以放到State.cs对应服务端的State.py
            这里请原谅我偷个懒， 全部放在逻辑实体基础对象了
        */
        public virtual void set_HP(object old)
        {
            object v = getDefinedProperty("HP");
            //          Dbg.DEBUG_MSG(className + "::set_HP: " + old + " => " + v); 
            Event.fireOut("set_HP", new object[] { this, v });
        }

        public virtual void set_MP(object old)
        {
            object v = getDefinedProperty("MP");
            //          Dbg.DEBUG_MSG(className + "::set_MP: " + old + " => " + v); 
            Event.fireOut("set_MP", new object[] { this, v });
        }

        public virtual void set_HP_Max(object old)
        {
            object v = getDefinedProperty("HP_Max");
            //          Dbg.DEBUG_MSG(className + "::set_HP_Max: " + old + " => " + v); 
            Event.fireOut("set_HP_Max", new object[] { this, v });
        }

        public virtual void set_MP_Max(object old)
        {
            object v = getDefinedProperty("MP_Max");
            //          Dbg.DEBUG_MSG(className + "::set_MP_Max: " + old + " => " + v); 
            Event.fireOut("set_MP_Max", new object[] { this, v });
        }

        public virtual void set_level(object old)
        {
            object v = getDefinedProperty("level");
            //          Dbg.DEBUG_MSG(className + "::set_level: " + old + " => " + v); 
            Event.fireOut("set_level", new object[] { this, v });
        }

        public virtual void set_name(object old)
        {
            object v = getDefinedProperty("name");
            //          Dbg.DEBUG_MSG(className + "::set_name: " + old + " => " + v); 
            Event.fireOut("set_name", new object[] { this, v });
        }

        public virtual void set_state(object old)
        {
            object v = getDefinedProperty("state");
            //          Dbg.DEBUG_MSG(className + "::set_state: " + old + " => " + v); 
            Event.fireOut("set_state", new object[] { this, v });
        }

        public virtual void set_subState(object old)
        {
            //          Dbg.DEBUG_MSG(className + "::set_subState: " + getDefinedProperty("subState")); 
        }

        public virtual void set_utype(object old)
        {
            //          Dbg.DEBUG_MSG(className + "::set_utype: " + getDefinedProperty("utype")); 
        }

        public virtual void set_uid(object old)
        {
            //          Dbg.DEBUG_MSG(className + "::set_uid: " + getDefinedProperty("uid")); 
        }

        public virtual void set_spaceUType(object old)
        {
            //          Dbg.DEBUG_MSG(className + "::set_spaceUType: " + getDefinedProperty("spaceUType")); 
        }

        public virtual void set_moveSpeed(object old)
        {
            object v = getDefinedProperty("moveSpeed");
            //          Dbg.DEBUG_MSG(className + "::set_moveSpeed: " + old + " => " + v); 
            Event.fireOut("set_moveSpeed", new object[] { this, v });
        }

        public virtual void set_modelScale(object old)
        {
            object v = getDefinedProperty("modelScale");
            //          Dbg.DEBUG_MSG(className + "::set_modelScale: " + old + " => " + v); 
            Event.fireOut("set_modelScale", new object[] { this, v });
        }

        public virtual void set_modelID(object old)
        {
            object v = getDefinedProperty("modelID");
            //          Dbg.DEBUG_MSG(className + "::set_modelID: " + old + " => " + v); 
            Event.fireOut("set_modelID", new object[] { this, v });
        }

        public virtual void set_forbids(object old)
        {
            //          Dbg.DEBUG_MSG(className + "::set_forbids: " + getDefinedProperty("forbids")); 
        }

        public virtual void recvDamage(Int32 attackerID, Int32 skillID, Int32 damageType, Int32 damage)
        {
            //          Dbg.DEBUG_MSG(className + "::recvDamage: attackerID=" + attackerID + ", skillID=" + skillID + ", damageType=" + damageType + ", damage=" + damage);

            Entity entity = KBEngineApp.app.findEntity(attackerID);

            Event.fireOut("recvDamage", new object[] { this, entity, skillID, damageType, damage });
        }
    }

}
