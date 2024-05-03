using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RoundStatus { Player, PC }

public class BlackJack : MonoBehaviour
{
    [Header("牌組GM")]
    public PokerGameManager GM;
    private RoundStatus roundStatus;

    [Header("系統元件")]
    public Button dealBtn;
    public Button passBtn;
    public Text winText;

    [Header("電腦的牌")]
    public List<Image> pcImages;
    public Text pcScoreText;
    private int pcScore;
    private List<PokerCard> pcCards = new List<PokerCard>();

    [Header("玩家的牌")]
    public List<Image> userImages;
    public Text userScoreText;
    private int userScore;
    private List<PokerCard> userCards = new List<PokerCard>();

    private bool isGameOver;
    private bool isWinner;
    private bool pcPass;
    private bool userPass;

    void Start()
    {
        //讓牌組先產生出來(延遲一秒)
        Invoke("GameStart", 1f);
    }

    /// <summary>
    /// 開始發牌(21點規則(發牌)
    /// </summary>
    public void GameStart()
    {
        //發牌兩輪
        for(int i = 0; i < 2; i++)
        {
            //GM發1張牌給玩家(1覆蓋但自己顯示1翻開)
            userCards.Add(GM.pokerStack.Pop());
            userImages[i].sprite = userCards[i].face;
            userImages[i].gameObject.SetActive(true);
            //GM發1張牌給電腦(1蓋1開)
            pcCards.Add(GM.pokerStack.Pop());
            if(i > 0)pcImages[i].sprite = pcCards[i].face;
            pcImages[i].gameObject.SetActive(true);
        }

        //玩家開始決策
        StatusChecker(RoundStatus.Player);
    }

    /// <summary>
    /// 玩家CALL牌
    /// </summary>
    public void Deal()
    {
        //發牌給玩家
        userCards.Add(GM.pokerStack.Pop());
        userImages[userCards.Count - 1].sprite = userCards[userCards.Count - 1].face;
        userImages[userCards.Count - 1].gameObject.SetActive(true);
        StatusChecker(RoundStatus.PC);
    }

    public void DealTOPC()
    {
        pcCards.Add(GM.pokerStack.Pop());
        pcImages[pcCards.Count - 1].sprite = pcCards[pcCards.Count - 1].face;
        pcImages[pcCards.Count - 1].gameObject.SetActive(true);
        StatusChecker(RoundStatus.Player);
    }

    /// <summary>
    /// 不在CALL牌，鎖定
    /// </summary>
    public void Pass()
    {
        userPass = true;
        passBtn.interactable = false;
        StatusChecker(RoundStatus.PC);
    }

    /// <summary>
    /// 遊戲狀態確認
    /// </summary>
    /// <param name="roundStatus">狀態機</param>
    void StatusChecker(RoundStatus roundStatus)
    {
        //遊戲是否還可以繼續
        if (GaameOver()) return;
            
        //PASS未按下，輪流操作
        if (passBtn.interactable)
        {
            this.roundStatus = roundStatus;
        }
        else 
        {
             this.roundStatus = RoundStatus.PC;
        }

            switch (this.roundStatus)
        {
            case RoundStatus.Player:
                dealBtn.interactable = true;
                passBtn.interactable = true;

                break;

            case RoundStatus.PC:
                dealBtn.interactable = false;
                //電腦AI判定是否CALL牌
                
                if (CardsChecker(pcCards) <= 17)
                {
                    DealTOPC();
                }
                else
                {
                    pcPass = true;
                    StatusChecker(RoundStatus.Player);
                }
               
                break;

        }
    }

    /// <summary>
    /// 21點的檢查規則
    /// </summary>
    /// <param name="pokerCards">整副牌組</param>
    /// 回傳結果
    int CardsChecker(List<PokerCard> pokerCards)
    {
        //牌組裡是否有A(無論1張2張都只能+10)
        bool getA = false;
        //全部數字總和(是否有A : 要不要+10)
        int totalPoint = 0;
        int currentPoint = 0;
        //牌組掃描
        foreach (PokerCard card in pokerCards)
        {
            currentPoint = card.BlackJack();
            if (currentPoint == 1) getA = true;
            totalPoint +=  currentPoint;

        }
       
        return getA ? ((21 - totalPoint) >= 10 ? totalPoint += 10 : totalPoint) : totalPoint;
    }

    bool GaameOver()
    {
        int pcPoint = CardsChecker(pcCards);
        int userPoint = CardsChecker(userCards);
        userScoreText.text = userPoint.ToString();
    

        //過五關遊戲結束
        if (pcCards.Count >= 5)
        {            
            isGameOver = true;
            isWinner = false;
        }
        if (userCards.Count >= 5)
        {
            isGameOver = true;
            isWinner = true;
        }
        
        //21點遊戲結束
        if (CardsChecker(pcCards) > 21 || CardsChecker(userCards) == 21)
        {
            isGameOver = true;
            isWinner = true;
        }
       else if (CardsChecker(pcCards) == 21 || CardsChecker(userCards) > 21)
        {
            isGameOver = true;
            isWinner = false;
        }

        //雙方都鎖定，直接比大小
        if(pcPass && userPass)
        {
            isGameOver = true;
            isWinner = userPoint > pcPoint;

        }
        
        if (isGameOver)
        {
            pcImages[0].sprite = pcCards[0].face;
            pcScoreText.text = pcPoint.ToString();
            dealBtn.interactable = false;
            passBtn.interactable = false;
            winText.text = isWinner ? "玩家勝利" : "電腦勝利";
            Debug.Log(isWinner ? "玩家贏" : "電腦贏");
        }

        return isGameOver;
    }

}
