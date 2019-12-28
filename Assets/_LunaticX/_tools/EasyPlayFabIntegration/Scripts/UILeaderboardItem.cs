namespace CurioAssets
{
    using UnityEngine;
    using UnityEngine.UI;
    //Class that holds UI Items for creating playfab leaderboard - Managed by EasyPlayFabIntegration
    public class UILeaderboardItem : MonoBehaviour
    {
        public Image picImg;
        public Text txtName, txtScore, txtRank; //Used to Store and Display Name, Score and Rank from the server
        public void AssignValues(int rank, string txtName, string txtScore)
        {
            gameObject.SetActive(true);
            if (rank > 3)
                txtRank.text = rank.ToString();

            this.txtName.text = txtName;
            this.txtScore.text = txtScore;
        }
    }
}