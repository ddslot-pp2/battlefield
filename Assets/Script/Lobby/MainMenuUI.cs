using System;
using UnityEngine;
using UnityEngine.Events;
using Tanks.Utilities;

namespace Tanks.UI
{
	//Page in menu to return to
	public enum MenuPage
	{
		Home,
		SinglePlayer,
		Lobby,
		CustomizationPage
	}

	//Class that handles main menu UI and transitions
	public class MainMenuUI : Singleton<MainMenuUI>
	{
		#region Static config

		public static MenuPage s_ReturnPage;

		#endregion

		#region Fields

		[SerializeField]
		protected CanvasGroup m_DefaultPanel;
		[SerializeField]
		protected CanvasGroup m_CreateGamePanel;
		[SerializeField]
		protected CanvasGroup m_LobbyPanel;
		[SerializeField]
		protected CanvasGroup m_SinglePlayerPanel;
		[SerializeField]
		protected CanvasGroup m_ServerListPanel;
		[SerializeField]
		protected CanvasGroup m_CustomizePanel;

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
				case MenuPage.Home:
				default:
					ShowDefaultPanel();
					break;
				case MenuPage.Lobby:
					ShowLobbyPanel();
					break;
				case MenuPage.CustomizationPage:
					ShowCustomizePanel();
					break;
				case MenuPage.SinglePlayer:
					ShowSingleplayerPanel();
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
			ShowPanel(m_DefaultPanel);
		}

		public void ShowLobbyPanel()
		{
			ShowPanel(m_LobbyPanel);
		}


		public void ShowServerListPanel()
		{
			ShowPanel(m_ServerListPanel);
		}
			
		public void ShowCustomizePanel()
		{
			ShowPanel(m_CustomizePanel);
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


		private void ShowSingleplayerPanel()
		{
			ShowPanel(m_SinglePlayerPanel);
		}

		private void GoToSingleplayerPanel()
		{
			ShowSingleplayerPanel();
		}

	
		#endregion


		#region Button events

		public void OnCustomiseClicked()
		{
			ShowCustomizePanel ();
		}

		public void OnCreateGameClicked()
		{
			//GoToCreateGamePanel ();
		}

		public void OnSinglePlayerClicked()
		{
			// Set network into SP mode
			GoToSingleplayerPanel();
		}

		public void OnFindGameClicked()
		{
			//GoToFindGamePanel ();
		}

		public void OnQuitGameClicked()
		{
			Application.Quit();
		}

		#endregion
	}
}