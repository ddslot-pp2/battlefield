public enum opcode : short
{
	CS_LOG_IN = 1000,
	SC_LOG_IN = 1001,
	CS_FIELD_LIST = 1002,
	SC_FIELD_LIST = 1003,
	CS_ENTER_FIELD = 1004,
	SC_ENTER_FIELD = 1005,
	CS_LEAVE_FIELD = 1006,
	SC_LEAVE_FIELD = 1007,
	CS_PURCHASE_CHARACTER = 1008,
	SC_PURCHASE_CHARACTER = 1009,
	CS_CHARACTER_INFO = 1010,
	SC_CHARACTER_INFO = 1011,
	CS_MY_INFO = 1012,
	SC_MY_INFO = 1013,
	CS_MY_CHARACTER_INFO = 1014,
	SC_MY_CHARACTER_INFO = 1015,
	CS_CHARACTER_UPGRADE = 1016,
	SC_CHARACTER_UPGRADE = 1017,
	CS_CHARACTER_SELECT = 1018,
	SC_CHARACTER_SELECT = 1019,
	CS_PING = 2000,
	SC_PING = 2001,
	CS_SYNC_FIELD = 2002,
	SC_SYNC_FIELD = 2003,
	SC_NOTI_OTHER_ENTER_FIELD = 2004,
	SC_NOTI_OTHER_LEAVE_FIELD = 2005,
	CS_NOTI_MOVE = 2006,
	SC_NOTI_OTHER_MOVE = 2007,
	CS_FIRE = 2008,
	SC_NOTI_FIRE = 2009,
	SC_NOTI_DESTROY_BULLET = 2010,
	SC_NOTI_DESTROY_CHARACTER = 2011,
	CS_RESPAWN_CHARACTER = 2012,
	SC_NOTI_RESPAWN_CHARACTER = 2013,
	SC_SELECT_BUFF = 2014,
	CS_ENHANCE_BUFF = 2015,
	SC_NOTI_UPDATE_CHARACTER_STATUS = 2016,
	SC_NOTI_ACTIVE_ITEM = 2017,
	SC_NOTI_ACQUIRE_ITEM = 2018,
	SC_NOTI_CREATE_MEDAL_ITEM = 2019,
	SC_NOTI_ACQUIRE_PERSIST_ITEM = 2020,
	SC_NOTI_RANK_INFO = 2021,
	SC_NOTI_RANK = 2022,
};
