using System;
using UnityEngine;
using UnityEngine.Events;
using Tanks.Utilities;

namespace Tanks.UI
{
	//Page in menu to return to
	public enum MenuPage
	{
		Lobby,
		Garage

	}

	//Class that handles main menu UI and transitions
	public class MainMenuUI : Singleton<MainMenuUI>
	{
		#region Static config

		public static MenuPage s_ReturnPage;

		#endregion

		#region Fields

		[SerializeField]
		protected CanvasGroup m_LobbyPanel;
		[SerializeField]
		protected CanvasGroup m_GaragePanel;
	
		[SerializeField]
		protected GameObject m_QuitButton;

		private CanvasGroup m_CurrentPanel;


		#endregion


		#region Methods


		protected virtual void Start()
		{
			//Used to return to correct page on return to menu
			switch (s_ReturnPage)
			{
				case MenuPage.Lobby:
				default:
					ShowLobbyPanel();
					break;

				case MenuPage.Garage:
					ShowGaragePanel();
					break;
			
			}
		}
		
		//Convenience function for showing panels
		public void ShowPanel(CanvasGroup newPanel)
		{
			if (m_CurrentPanel != null)
			{
				m_CurrentPanel.gameObject.SetActive(false);
			}

			m_CurrentPanel = newPanel;
			if (m_CurrentPanel != null)
			{
				m_CurrentPanel.gameObject.SetActive(true);
			}
		}

		public void ShowDefaultPanel()
		{
			ShowPanel(m_LobbyPanel);
		}

		public void ShowLobbyPanel()
		{
			ShowPanel(m_LobbyPanel);
		}


		public void ShowGaragePanel()
		{
			ShowPanel(m_GaragePanel);
		}
	

		/*
		public void ShowInfoPopup(string label, UnityAction callback)
		{
			if (m_InfoPanel != null)
			{
				//m_InfoPanel.Display(label, callback, true);
			}
		}

		public void ShowInfoPopup(string label)
		{
			if (m_InfoPanel != null)
			{
				//m_InfoPanel.Display(label, null, false);
			}
		}


		public void HideInfoPopup()
		{
			if (m_InfoPanel != null)
			{
				//m_InfoPanel.gameObject.SetActive(false);
			}
		}
		*/


	

	
		#endregion


		#region Button events

		public void OnQuitGameClicked()
		{
			Application.Quit();
		}

		#endregion
	}
}