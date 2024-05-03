using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RoundStatus { Player, PC }

public class BlackJack : MonoBehaviour
{
    [Header("�P��GM")]
    public PokerGameManager GM;
    private RoundStatus roundStatus;

    [Header("�t�Τ���")]
    public Button dealBtn;
    public Button passBtn;
    public Text winText;

    [Header("�q�����P")]
    public List<Image> pcImages;
    public Text pcScoreText;
    private int pcScore;
    private List<PokerCard> pcCards = new List<PokerCard>();

    [Header("���a���P")]
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
        //���P�ե����ͥX��(����@��)
        Invoke("GameStart", 1f);
    }

    /// <summary>
    /// �}�l�o�P(21�I�W�h(�o�P)
    /// </summary>
    public void GameStart()
    {
        //�o�P���
        for(int i = 0; i < 2; i++)
        {
            //GM�o1�i�P�����a(1�л\���ۤv���1½�})
            userCards.Add(GM.pokerStack.Pop());
            userImages[i].sprite = userCards[i].face;
            userImages[i].gameObject.SetActive(true);
            //GM�o1�i�P���q��(1�\1�})
            pcCards.Add(GM.pokerStack.Pop());
            if(i > 0)pcImages[i].sprite = pcCards[i].face;
            pcImages[i].gameObject.SetActive(true);
        }

        //���a�}�l�M��
        StatusChecker(RoundStatus.Player);
    }

    /// <summary>
    /// ���aCALL�P
    /// </summary>
    public void Deal()
    {
        //�o�P�����a
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
    /// ���bCALL�P�A��w
    /// </summary>
    public void Pass()
    {
        userPass = true;
        passBtn.interactable = false;
        StatusChecker(RoundStatus.PC);
    }

    /// <summary>
    /// �C�����A�T�{
    /// </summary>
    /// <param name="roundStatus">���A��</param>
    void StatusChecker(RoundStatus roundStatus)
    {
        //�C���O�_�٥i�H�~��
        if (GaameOver()) return;
            
        //PASS�����U�A���y�ާ@
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
                //�q��AI�P�w�O�_CALL�P
                
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
    /// 21�I���ˬd�W�h
    /// </summary>
    /// <param name="pokerCards">��ƵP��</param>
    /// �^�ǵ��G
    int CardsChecker(List<PokerCard> pokerCards)
    {
        //�P�ո̬O�_��A(�L��1�i2�i���u��+10)
        bool getA = false;
        //�����Ʀr�`�M(�O�_��A : �n���n+10)
        int totalPoint = 0;
        int currentPoint = 0;
        //�P�ձ��y
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
    

        //�L�����C������
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
        
        //21�I�C������
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

        //���賣��w�A������j�p
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
            winText.text = isWinner ? "���a�ӧQ" : "�q���ӧQ";
            Debug.Log(isWinner ? "���aĹ" : "�q��Ĺ");
        }

        return isGameOver;
    }

}
