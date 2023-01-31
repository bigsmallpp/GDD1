using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //enum View {currPauseView, currControlView};
    public GameObject pauseView;
    public GameObject controlView;
    //private GameObject[] _views;
    //private int _view_count = 0;
    //private View _curr_view;
    void Start()
    {
        //_view_count = 0;
        //_curr_view = currPauseView;
        //_views[0] = pauseView;
        //_views[1] = controlView;
    }

    //direction true for forward, false for backward
    /*public void openView(bool direction)
    {
        _views[_view_count].SetActive(false);
        if (direction)
        {
            _view_count++;
        }
        else
        {
            _view_count--;
        }
        _views[_view_count].SetActive(true);
    }*/

    public void openControlView()
    {
        pauseView.SetActive(false);
        controlView.SetActive(true);
    }

    public void backOut()
    {
        controlView.SetActive(false);
        pauseView.SetActive(true);
    }
}
