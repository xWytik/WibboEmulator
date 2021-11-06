﻿namespace Butterfly.Communication.Packets.Outgoing
{
    public static class ServerPacketHeader //PRODUCTION-201611291003-a-1161735
    {
        public const int RoomUserQuestionAnsweredComposer = 2589;
        public const int QuestionInfoComposer = 2665;
        public const int SimplePollAnswersComposer = 1066;
        public const int CameraPurchaseSuccesfullComposer = 2783;
        public const int InitCryptoMessageComposer = 1347;
        public const int SecretKeyMessageComposer = 3885;
        public const int NuxAlertComposer = 1243;
        public const int HelperToolMessageComposer = 1548;
        public const int OnGuideSessionStarted = 3209;
        public const int OnGuideSessionPartnerIsTyping = 1016;
        public const int OnGuideSessionMsg = 841;
        public const int OnGuideSessionInvitedToGuideRoom = 219;
        public const int OnGuideSessionRequesterRoom = 1847;
        public const int OnGuideSessionEnded = 1456;
        public const int OnGuideSessionAttached = 1591;
        public const int OnGuideSessionDetached = 138;
        public const int OnGuideSessionError = 673;
        public const int SellablePetBreedsMessageComposer = 3331;
        public const int CheckPetNameMessageComposer = 1503;
        public const int AchievementUnlockedMessageComposer = 806;
        public const int ModToolIssueResponseAlertComposer = 3796;
        public const int PetTrainingPanelMessageComposer = 1164;
        public const int OpenHelpToolMessageComposer = 1121;
        public const int CanCreateRoomMessageComposer = 378;
        public const int GroupMembershipRequestedMessageComposer = 1180;
        public const int UpdateFavouriteGroupMessageComposer = 3403;
        public const int NameChangeUpdateMessageComposer = 563;
        public const int RefreshFavouriteGroupMessageComposer = 876;
        public const int NewGroupInfoMessageComposer = 2808;
        public const int ModeratorTicketChatlogMessageComposer = 607;
        public const int ModeratorSupportTicketMessageComposer = 3609;
        public const int ModeratorSupportTicketResponseMessageComposer = 3796; //new
        public const int RespectPetNotificationMessageComposer = 2788;
        public const int PetLevelUpComposer = 859;
        public const int AddExperiencePointsMessageComposer = 2156;
        public const int GroupFurniSettingsMessageComposer = 3293;
        public const int QuestListMessageComposer = 3625;
        public const int QuestStartedMessageComposer = 230;
        public const int QuestAbortedMessageComposer = 3027;
        public const int QuestCompletedMessageComposer = 949;

        public const int ACHIEVEMENT_PROGRESSED = 2107;
        public const int MARKETPLACE_ITEMS_SEARCHED = 680;
        public const int MARKETPLACE_CANCEL_SALE = 3264;
        public const int MARKETPLACE_ITEM_POSTED = 1359;
        public const int MARKETPLACE_SELL_ITEM = 54;
        public const int MARKETPLACE_ITEM_STATS = 725;
        public const int MARKETPLACE_OWN_ITEMS = 3884;
        public const int GROUP_BADGE_PARTS = 2238;
        public const int IN_CLIENT_LINK = 2023;
        public const int MESSENGER_INSTANCE_MESSAGE_ERROR = 3359;
        public const int UNIT_NUMBER = 2324;
        public const int MODTOOL_ROOM_INFO = 1333;
        public const int MODTOOL_ROOM_CHATLOG = 3434;
        public const int MODTOOL_USER_CHATLOG = 3377;
        public const int BOT_COMMAND_CONFIGURATION = 1618;
        public const int REDEEM_VOUCHER_ERROR = 714;
        public const int LOVELOCK_FURNI_FINISHED = 770;
        public const int LOVELOCK_FURNI_FRIEND_COMFIRMED = 382;
        public const int LOVELOCK_FURNI_START = 3753;
        public const int PLAYING_GAME = 448;
        public const int CAMERA_PRICE = 3878;
        public const int CAMERA_URL = 3696;
        public const int AUTHENTICATED = 2491;
        public const int USER_INFO = 2725;
        public const int USER_PERKS = 2586;
        public const int USER_PERMISSIONS = 411;
        public const int USER_CHANGE_NAME = 118;
        public const int USER_CURRENCY_UPDATE = 2275;
        public const int GENERIC_ERROR = 1600;
        public const int SECURITY_MACHINE = 1488;
        public const int AVAILABILITY_STATUS = 2033;
        public const int BUILDERS_CLUB_EXPIRED = 1452;
        public const int USER_OUTFITS = 3315;
        public const int USER_FAVORITE_ROOM_COUNT = 151;
        public const int USER_EFFECTS = 340;
        public const int USER_HOME_ROOM = 2875;
        public const int NAVIGATOR_LIFTED = 3104;
        public const int NAVIGATOR_SETTINGS = 518;
        public const int NAVIGATOR_EVENT_CATEGORIES = 3244;
        public const int NAVIGATOR_METADATA = 3052;
        public const int NAVIGATOR_COLLAPSED = 1543;
        public const int NAVIGATOR_SEARCH = 2690;
        public const int MESSENGER_INIT = 1605;
        public const int MESSENGER_UPDATE = 2800;
        public const int MESSENGER_FRIENDS = 3130;
        public const int MESSENGER_RELATIONSHIPS = 2016;
        public const int MESSENGER_REQUESTS = 280;
        public const int MESSENGER_REQUEST = 2219;
        public const int MESSENGER_CHAT = 1587;
        public const int MESSENGER_ROOM_INVITE = 3870;
        public const int MESSENGER_SEARCH = 973;
        public const int USER_CURRENCY = 2018;
        public const int USER_CREDITS = 3475;
        public const int MODERATION_TOOL = 2696;
        public const int MODERATION_TOPICS = 325;
        public const int ROOM_MODEL_DOOR = 1664;
        public const int ROOM_MODEL_BLOCKED_TILES = 3990;
        public const int DESKTOP_CAMPAIGN = 1745;
        public const int DESKTOP_NEWS = 286;
        public const int USER_SETTINGS = 513;
        public const int USER_SUBSCRIPTION = 954;
        public const int CATALOG_SEARCH = 3388;
        public const int FURNITURE_ALIASES = 1723;
        public const int GIFT_CONFIG = 2234;
        public const int ROOM_SETTINGS_CHAT = 1191;
        public const int DISCOUNT_CONFIG = 2347;
        public const int CATALOG_MODE = 3828;
        public const int CLIENT_LATENCY = 10;
        public const int USER_FAVORITE_ROOM = 2524;
        public const int USER_PROFILE = 3898;
        public const int CATALOG_PAGES = 1032;
        public const int CATALOG_PAGE = 804;
        public const int CATALOG_UPDATED = 1866;
        public const int USER_CLOTHING = 1450;
        public const int ROOM_BAN_REMOVE = 3429;
        public const int ACHIEVEMENT_LIST = 305;
        public const int USER_ACHIEVEMENT_SCORE = 1968;
        public const int GENERIC_ALERT = 3801;
        public const int NOTIFICATION_LIST = 1992;
        public const int GENERIC_ALERT_MESSAGES = 2035;
        public const int PET_FIGURE_UPDATE = 1924;
        public const int USER_FURNITURE_REFRESH = 3151;
        public const int CATALOG_PURCHASE = 869;
        public const int UNSEEN_ITEMS = 2103;
        public const int ROOM_CREATED = 1304;
        public const int ROOM_SETTINGS = 1498;
        public const int ROOM_SETTINGS_SAVE = 948;
        public const int ROOM_ENTER_ERROR = 899;
        public const int DESKTOP_VIEW = 122;
        public const int ROOM_DOORBELL_DENIED = 878;
        public const int ROOM_DOORBELL_ADD = 2309;
        public const int ROOM_DOORBELL_CLOSE = 3783;
        public const int ROOM_MODEL_NAME = 2031;
        public const int ROOM_FORWARD = 160;
        public const int TRADE = 2505;
        public const int TRADE_UPDATE = 2024;
        public const int TRADE_ACCEPTED = 2568;
        public const int TRADE_CONFIRM = 2720;
        public const int TRADE_CLOSE = 1001;
        public const int TRADE_CLOSED = 1373;
        public const int ROOM_PAINT = 2454;
        public const int ROOM_RIGHTS = 780;
        public const int ROOM_RIGHTS_OWNER = 339;
        public const int ROOM_RIGHTS_CLEAR = 2392;
        public const int ROOM_SCORE = 482;
        public const int ROOM_MODEL = 1301;
        public const int ROOM_HEIGHT_MAP = 2753;
        public const int FURNITURE_FLOOR = 1778;
        public const int ITEM_WALL = 1369;
        public const int UNIT = 374;
        public const int ROOM_THICKNESS = 3547;
        public const int ROOM_INFO_OWNER = 749;
        public const int ROOM_INFO = 687;
        public const int UNIT_STATUS = 1640;
        public const int NAVIGATOR_CATEGORIES = 1562;
        public const int UNIT_FLOOD_CONTROL = 566;
        public const int UNIT_CHAT = 1446;
        public const int UNIT_CHAT_SHOUT = 1036;
        public const int UNIT_CHAT_WHISPER = 2704;
        public const int USER_RESPECT = 2815;
        public const int UNIT_INFO = 3920;
        public const int UNIT_DANCE = 2233;
        public const int UNIT_EXPRESSION = 1631;
        public const int UNIT_IDLE = 1797;
        public const int USER_FURNITURE = 994;
        public const int USER_FURNITURE_ADD = 104;
        public const int USER_PETS = 3522;
        public const int PET_INFO = 2901;
        public const int USER_BADGES = 717;
        public const int USER_BADGES_CURRENT = 1087;
        public const int USER_BADGES_ADD = 2493;
        public const int ROOM_RIGHTS_LIST_ADD = 2088;
        public const int ROOM_RIGHTS_LIST_REMOVE = 1327;
        public const int ROOM_RIGHTS_LIST = 1284;
        public const int ROOM_BAN_LIST = 1869;
        public const int UNIT_TYPING = 1717;
        public const int USER_FURNITURE_REMOVE = 159;
        public const int FURNITURE_FLOOR_ADD = 1534;
        public const int ITEM_WALL_ADD = 2187;
        public const int FURNITURE_FLOOR_UPDATE = 3776;
        public const int ITEM_WALL_UPDATE = 2009;
        public const int FURNITURE_FLOOR_REMOVE = 2703;
        public const int FURNITURE_OPEN_GIFT = 56;
        public const int ITEM_WALL_REMOVE = 3208;
        public const int WIRED_TRIGGER = 383;
        public const int WIRED_ACTION = 1434;
        public const int WIRED_CONDITION = 1108;
        public const int WIRED_SAVE = 1155;
        public const int UNIT_REMOVE = 2661;
        public const int MODERATION_USER_INFO = 2866;
        public const int UNIT_EFFECT = 1167;
        public const int UNIT_HAND_ITEM = 1474;
        public const int ROOM_ROLLING = 3207;
        public const int ITEM_DIMMER_SETTINGS = 2710;
        public const int FURNITURE_ITEMDATA = 2202;
        public const int UNIT_CHANGE_NAME = 2182;
        public const int ROOM_SETTINGS_UPDATED = 3297;
        public const int GROUP_MEMBERS_REFRESH = 2445;
        public const int GROUP_CREATE_OPTIONS = 2159;
        public const int GROUP_LIST = 420;
        public const int GROUP_BADGES = 2402;
        public const int GROUP_INFO = 1702;
        public const int GROUP_SETTINGS = 3965;
        public const int GROUP_MEMBERS = 1200;
        public const int GROUP_MEMBER = 265;
        public const int USER_BOTS = 3086;
        public const int MARKETPLACE_CONFIG = 1823;
        public const int USER_IGNORED = 126;
        public const int USER_IGNORED_UPDATE = 207;
    }
}
