using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class PokerGameManager : MonoBehaviour
{
    [Header("�~�P")]
    public bool isShuffle = true;
    /// <summary>
    /// ��ƹ�����(�~�P�L)
    /// </summary>
    private List <PokerCard> pokerCards;
    /// <summary>
    /// �Ȧs(���~�L)
    /// </summary>
    private List<PokerCard> pokerTmp;
    /// <summary>
    /// ���θ�ƪ���~���f
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


    [Header("���ե�UI�v������")]
    public Image cardImage;

    [Header("�]�w���P�ݨD��")]
    public SetJoker setJoker;

    
    void Start()
    {
        if (!isShuffle)
        {
            //���~�P
            pokerCards = SystemStatus.GetCards(setJoker);
            //���פ����޿�
            return;
        }
        //----------------�~�P�޿�϶�-----------------
        //�ƻs����P�ո��(�]�w�O�_���P)�P�ո�ƨ�Ȧs
        pokerTmp = SystemStatus.GetCards(setJoker);
        //�}�l�~�P:�H������@�i����|��
        pokerCards = new List<PokerCard>();
        //�u�n�٦��Ѿl�P�A�~����(�~) Count(�ƶq)
        while (pokerTmp.Count > 0)
        {

            int index = Random.Range(0, pokerTmp.Count);
            //�H������@�i���
            PokerCard card = pokerTmp[index];
            pokerCards.Add(card);
            pokerTmp.Remove(card);
        }



    }

   
    
    /// <summary>
    /// �~�L����P
    /// </summary>
    public void GetCard()
    {
        if (pokerStack.Count > 0) cardImage.sprite = pokerStack.Pop().face;
    }
    /// <summary>
    /// ��d�\��
    /// </summary>
    /// <param name="B">�O�_�q�Ĥ@�i���</param>
    public void GetCard(bool B)
    {

        if(B)
        {
            //�ƶ�����覡
            if (pokerQueue.Count > 0)cardImage.sprite = pokerQueue.Dequeue().face;
        }
        else
        {
            //���|����覡
            if (pokerStack.Count > 0) cardImage.sprite = pokerStack.Pop().face;
        }
    }
}
