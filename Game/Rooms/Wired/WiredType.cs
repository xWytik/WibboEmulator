﻿namespace Butterfly.Game.Rooms.Wired
{
    internal class WiredType
    {
        public enum WiredConditionType
        {
            STATES_MATCH = 0,
            FURNIS_HAVE_AVATARS = 1,
            TRIGGERER_IS_ON_FURNI = 2,
            TIME_ELAPSED_MORE = 3,
            TIME_ELAPSED_LESS = 4,
            USER_COUNT_IN = 5,
            ACTOR_IS_IN_TEAM = 6,
            HAS_STACKED_FURNIS = 7,
            STUFF_TYPE_MATCHES = 8,
            STUFFS_IN_FORMATION = 9,
            ACTOR_IS_GROUP_MEMBER = 10,
            ACTOR_IS_WEARING_BADGE = 11,
            ACTOR_IS_WEARING_EFFECT = 12,
            NOT_STATES_MATCH = 13,
            FURNI_NOT_HAVE_HABBO = 14,
            NOT_ACTOR_ON_FURNI = 15,
            NOT_USER_COUNT_IN = 16,
            NOT_ACTOR_IN_TEAM = 17,
            NOT_HAS_STACKED_FURNIS = 18,
            NOT_FURNI_IS_OF_TYPE = 19,
            NOT_STUFFS_IN_FORMATION = 20,
            NOT_ACTOR_IN_GROUP = 21,
            NOT_ACTOR_WEARS_BADGE = 22,
            NOT_ACTOR_WEARING_EFFECT = 23,
            DATE_RANGE_ACTIVE = 24,
            ACTOR_HAS_HANDITEM = 25,
        }

        public enum WiredActionType
        {
            TOGGLE_FURNI_STATE = 0,
            RESET = 1,
            SET_FURNI_STATE = 3,
            MOVE_FURNI = 4,
            GIVE_SCORE = 6,
            CHAT = 7,
            TELEPORT = 8,
            JOIN_TEAM = 9,
            LEAVE_TEAM = 10,
            CHASE = 11,
            FLEE = 12,
            MOVE_TO_DIRECTION = 13,
            GIVE_SCORE_TO_PREDEFINED_TEAM = 14,
            TOGGLE_TO_RANDOM_STATE = 15,
            MOVE_FURNI_TO = 16,
            GIVE_REWARD = 17,
            CALL_ANOTHER_STACK = 18,
            KICK_FROM_ROOM = 19,
            MUTE_USER = 20,
            BOT_TELEPORT = 21,
            BOT_MOVE = 22,
            BOT_TALK = 23,
            BOT_GIVE_HAND_ITEM = 24,
            BOT_FOLLOW_AVATAR = 25,
            BOT_CHANGE_FIGURE = 26,
            BOT_TALK_DIRECT_TO_AVTR = 27,
        }

        public enum WiredTriggerType
        {
            AVATAR_SAYS_SOMETHING = 0,
            AVATAR_WALKS_ON_FURNI = 1,
            AVATAR_WALKS_OFF_FURNI = 2,
            TRIGGER_ONCE = 3,
            TOGGLE_FURNI = 4,
            TRIGGER_PERIODICALLY = 6,
            AVATAR_ENTERS_ROOM = 7,
            GAME_STARTS = 8,
            GAME_ENDS = 9,
            SCORE_ACHIEVED = 10,
            COLLISION = 11,
            TRIGGER_PERIODICALLY_LONG = 12,
            BOT_REACHED_STUFF = 13,
            BOT_REACHED_AVATAR = 14,
        }
    }
}
