
namespace Assets.Scripts.GamePlay
{
    class ownVar
    {
        public static int finishedLevels = 0;
        public static int myID;
        public static int Turn=1;
        public static int MaxPlayers = 2;
        public static int[] OnField = new int[MaxPlayers];

        public static bool yourTurn()
        {
            if (Turn == myID)
            {
                return true;
            }
            return false;
        }
        public static void isFirstPlayer()
        {
            if (MaxPlayers == Turn)
            {
                Turn = 1;
            }
        }
    }
}
