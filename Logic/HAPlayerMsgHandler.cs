using System;
using CyberServer;

public partial class MsgHandler
{
    public static void HAMsgPlayerInfoUpload(ClientState c, MsgBase msgBase)
    {
        HAMsgPlayerInfoUpload msg = (HAMsgPlayerInfoUpload)msgBase;

        // 玩家信息为空
        if (DBManager.SelectPlayerInfo(msg.playerInfo._id) == null)
        {
            // 插入玩家数据
            if (DBManager.InsertPlayerInfo(msg.playerInfo._id, msg.playerInfo))
            {
                // 成功
                msg.result = 0;
                c.player = DBManager.SelectPlayerInfo(msg.playerInfo._id);
                LogService.Info("[服务器] 用户信息插入成功, " + c.socket.RemoteEndPoint);
            }
            else
            {
                // 失败
                msg.result = 1;
                LogService.Error("[服务器] 用户信息插入失败, " + c.socket.RemoteEndPoint);
            }

            NetManager.Send(c, msg);
            return;
        }

        // 玩家信息不为空，更新信息
        if (!DBManager.UpdatePlayerInfo(msg.playerInfo._id, msg.playerInfo))
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] 用户信息更新失败, " + c.socket.RemoteEndPoint);
            return;
        }

        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] 用户信息更新成功, " + c.socket.RemoteEndPoint);
    }

    public static void HAMsgPlayerInfoLoad(ClientState c, MsgBase msgBase)
    {
        HAMsgPlayerInfoLoad msg = (HAMsgPlayerInfoLoad)msgBase;

        msg.playerInfo = DBManager.SelectPlayerInfo(msg.playerInfo._id);

        if (msg.playerInfo == null)
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] 用户信息获取失败, " + c.socket.RemoteEndPoint);
            return;
        }

        c.player = DBManager.SelectPlayerInfo(msg.playerInfo._id);
        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] 用户信息获取成功, " + c.socket.RemoteEndPoint);
    }

    public static void MsgPlayerBaseLoad(ClientState c, MsgBase msgBase)
    {
        MsgPlayerBaseLoad msg = (MsgPlayerBaseLoad)msgBase;

        msg.playerBaseEntity = DBManager.SelectPlayerBase(msg.playerBaseEntity.id);

        if (msg.playerBaseEntity == null)
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] player_base 获取失败, " + c.socket.RemoteEndPoint);
            return;
        }

        if (c.player == null) c.player = new PlayerInfo();
        UpdatePlayerInfoByPlayerBase(c, msg.playerBaseEntity);

        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] player_base 获取成功, " + c.socket.RemoteEndPoint);
    }

    public static void MsgPlayerBaseSave(ClientState c, MsgBase msgBase)
    {
        MsgPlayerBaseSave msg = (MsgPlayerBaseSave)msgBase;

        if (DBManager.SelectPlayerBase(msg.playerBaseEntity.id) == null) // 玩家信息为空
        {
            if (DBManager.InsertPlayerBase(msg.playerBaseEntity)) // 插入玩家基础信息
            {
                msg.result = 0;
                UpdatePlayerInfoByPlayerBase(c, msg.playerBaseEntity);
                LogService.Info("[服务器] player_base 插入成功, " + c.socket.RemoteEndPoint);
            }
            else
            {
                msg.result = 1;
                LogService.Error("[服务器] player_base 插入失败, " + c.socket.RemoteEndPoint);
            }

            NetManager.Send(c, msg);
        }

        if (!DBManager.UpdatePlayerBase(msg.playerBaseEntity)) // 玩家信息不为空, 更新信息
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] player_base 更新失败, " + c.socket.RemoteEndPoint);
            return;
        }

        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] player_base 更新成功, " + c.socket.RemoteEndPoint);
    }

    public static void MsgPlayerStatsLoad(ClientState c, MsgBase msgBase)
    {
        MsgPlayerStatsLoad msg = (MsgPlayerStatsLoad)msgBase;

        msg.playerStatsEntity = DBManager.SelectPlayerStats(msg.playerStatsEntity.player_id);

        if (msg.playerStatsEntity == null)
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] player_stats 获取失败, " + c.socket.RemoteEndPoint);
            return;
        }

        if (c.player == null) c.player = new PlayerInfo();
        UpdatePlayerInfoByPlayerStats(c, msg.playerStatsEntity);

        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] player_stats 获取成功, " + c.socket.RemoteEndPoint);
    }

    public static void MsgPlayerStatsSave(ClientState c, MsgBase msgBase)
    {
        MsgPlayerStatsSave msg = (MsgPlayerStatsSave)msgBase;

        if (DBManager.SelectPlayerStats(msg.playerStatsEntity.player_id) == null)
        {
            if (DBManager.InsertPlayerStats(msg.playerStatsEntity))
            {
                msg.result = 0;
                UpdatePlayerInfoByPlayerStats(c, msg.playerStatsEntity);
                LogService.Info("[服务器] player_stats 插入成功, " + c.socket.RemoteEndPoint);
            }
            else
            {
                msg.result = 1;
                LogService.Error("[服务器] player_stats 插入失败, " + c.socket.RemoteEndPoint);
            }

            NetManager.Send(c, msg);
        }

        if (!DBManager.UpdatePlayerStats(msg.playerStatsEntity))
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] player_stats 更新失败, " + c.socket.RemoteEndPoint);
            return;
        }

        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] player_stats 更新成功, " + c.socket.RemoteEndPoint);
    }

    public static void MsgPlayerInventoryLoad(ClientState c, MsgBase msgBase)
    {
        MsgPlayerInventoryLoad msg = (MsgPlayerInventoryLoad)msgBase;

        msg.playerInventoryEntity = DBManager.SelectPlayerInventory(msg.playerInventoryEntity.player_id);

        if (msg.playerInventoryEntity == null)
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] player_inventory 获取失败, " + c.socket.RemoteEndPoint);
            return;
        }

        if (c.player == null) c.player = new PlayerInfo();
        UpdatePlayerInfoByPlayerInventory(c, msg.playerInventoryEntity);

        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] player_inventory 获取成功, " + c.socket.RemoteEndPoint);
    }

    public static void MsgPlayerInventorySave(ClientState c, MsgBase msgBase)
    {
        MsgPlayerInventorySave msg = (MsgPlayerInventorySave)msgBase;

        if (DBManager.SelectPlayerInventory(msg.playerInventoryEntity.player_id) == null)
        {
            if (DBManager.InsertPlayerInventory(msg.playerInventoryEntity))
            {
                msg.result = 0;
                UpdatePlayerInfoByPlayerInventory(c, msg.playerInventoryEntity);
                LogService.Info("[服务器] player_inventory 插入成功, " + c.socket.RemoteEndPoint);
            }
            else
            {
                msg.result = 1;
                LogService.Error("[服务器] player_inventory 插入失败, " + c.socket.RemoteEndPoint);
            }

            NetManager.Send(c, msg);
            return;
        }

        if (!DBManager.UpdatePlayerInventory(msg.playerInventoryEntity))
        {
            msg.result = 1;
            NetManager.Send(c, msg);
            LogService.Error("[服务器] player_inventory 更新失败, " + c.socket.RemoteEndPoint);
            return;
        }

        msg.result = 0;
        NetManager.Send(c, msg);
        LogService.Info("[服务器] player_inventory 更新成功, " + c.socket.RemoteEndPoint);
    }

    #region 辅助方法
    private static void UpdatePlayerInfoByPlayerBase(ClientState c, PlayerBaseEntity entity)
    {
        c.player._id = entity.id;
        c.player._name = entity.name;
        c.player._head = entity.head;
        c.player._level = entity.level;
        c.player._commonCurrency = entity.common_currency;
        c.player._rareCurrency = entity.rare_currency;
    }

    private static void UpdatePlayerInfoByPlayerStats(ClientState c, PlayerStatsEntity entity)
    {
        c.player._maxHP = entity.max_hp;
        c.player._maxMP = entity.max_mp;
        c.player._maxEXP = entity.max_exp;
        c.player._currentHP = entity.current_hp;
        c.player._currentMP = entity.current_mp;
        c.player._currentEXP = entity.current_exp;
        c.player._pAttack = entity.attack;
        c.player._pArmorPenetration = entity.armor_penetration;
        c.player._pDefense = entity.defense;
        c.player._pDamageAvoidance = entity.damage_avoidance;
        c.player._pCriticalProbability = entity.critical_probability;
        c.player._pCriticalMultiplier = entity.critical_multiplier;
        c.player._pSuckProbability = entity.suck_probability;
        c.player._pSuckMultiplier = entity.suck_multiplier;
    }

    private static void UpdatePlayerInfoByPlayerInventory(ClientState c, PlayerInventoryEntity entity)
    {
        c.player._allItems = entity.items;
        c.player._nowEquips = entity.now_equips;
        c.player._inventoryItemNum = entity.inventory_num;
        c.player._safeboxNum = entity.safebox_num;
    }
    #endregion
}