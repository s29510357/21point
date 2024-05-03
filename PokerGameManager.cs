using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class PokerGameManager : MonoBehaviour
{
    [Header("洗牌")]
    public bool isShuffle = true;
    /// <summary>
    /// 資料實體資料(洗牌過)
    /// </summary>
    private List <PokerCard> pokerCards;
    /// <summary>
    /// 暫存(未洗過)
    /// </summary>
    private List<PokerCard> pokerTmp;
    /// <summary>
    /// 取用資料的對外接口
    /// </summary>
    public List<PokerCard> pokerList
    {
        get { return pokerCards; }
    }
    private Stack<PokerCard> _pokerStack;
    public Stack<PokerCard> pokerStack 
    {
        get
        {
            if (_pokerStack == null)_pokerStack = new Stack<PokerCard>(pokerCards);
            return _pokerStack;
        }
    }
    private Queue<PokerCard> _pokerQueue;

    public Queue<PokerCard> pokerQueue
    {
        get
        {
            if (_pokerQueue == null) _pokerQueue = new Queue<PokerCard>(pokerCards);
            return _pokerQueue; 
        }
    }


    [Header("測試用UI影像元件")]
    public Image cardImage;

    [Header("設定鬼牌需求數")]
    public SetJoker setJoker;

    
    void Start()
    {
        if (!isShuffle)
        {
            //不洗牌
            pokerCards = SystemStatus.GetCards(setJoker);
            //阻擋之後邏輯
            return;
        }
        //----------------洗牌邏輯區塊-----------------
        //複製完整牌組資料(設定是否鬼牌)牌組資料到暫存
        pokerTmp = SystemStatus.GetCards(setJoker);
        //開始洗牌:隨機抽取一張到堆疊內
        pokerCards = new List<PokerCard>();
        //只要還有剩餘牌，繼續抽取(洗) Count(數量)
        while (pokerTmp.Count > 0)
        {

            int index = Random.Range(0, pokerTmp.Count);
            //隨機抽取一張資料
            PokerCard card = pokerTmp[index];
            pokerCards.Add(card);
            pokerTmp.Remove(card);
        }



    }

   
    
    /// <summary>
    /// 洗過的抽牌
    /// </summary>
    public void GetCard()
    {
        if (pokerStack.Count > 0) cardImage.sprite = pokerStack.Pop().face;
    }
    /// <summary>
    /// 抽卡功能
    /// </summary>
    /// <param name="B">是否從第一張抽取</param>
    public void GetCard(bool B)
    {

        if(B)
        {
            //排隊抽取方式
            if (pokerQueue.Count > 0)cardImage.sprite = pokerQueue.Dequeue().face;
        }
        else
        {
            //堆疊抽取方式
            if (pokerStack.Count > 0) cardImage.sprite = pokerStack.Pop().face;
        }
    }
}
