#include "stdafx.h"
#include "Volume.h"

#include "VolumeDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CVolumeDlg dialog

CVolumeDlg::CVolumeDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CVolumeDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CVolumeDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CVolumeDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CVolumeDlg)
	DDX_Control(pDX, IDOK, m_btnOK);
	DDX_Control(pDX, IDC_VOLUME, m_ctlVolume);
	DDX_Control(pDX, IDC_SLIDER_VOLUME, m_sliderVolume);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CVolumeDlg, CDialog)
	//{{AFX_MSG_MAP(CVolumeDlg)
	ON_WM_HSCROLL()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CVolumeDlg message handlers

BOOL CVolumeDlg::OnInitDialog()
{
    DWORD   dwVolume;
    CString strVolume;


	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

    m_sliderVolume.SetRange(0, 65535);

    dwVolume = GetVolume();
    m_sliderVolume.SetPos(dwVolume);

    strVolume.Format("%lu", dwVolume);
    m_ctlVolume.SetWindowText(strVolume);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

//====================================================================

void CVolumeDlg::SetVolume( const DWORD dwVolume )
{
    MMRESULT                        result;
    HMIXER                          hMixer;
    MIXERLINE                       ml   = {0};
    MIXERLINECONTROLS               mlc  = {0};
    MIXERCONTROL                    mc   = {0};
    MIXERCONTROLDETAILS             mcd  = {0};
    MIXERCONTROLDETAILS_UNSIGNED    mcdu = {0};


    // get a handle to the mixer device
    result = mixerOpen(&hMixer, MIXER_OBJECTF_MIXER, 0, 0, 0);
    if (MMSYSERR_NOERROR == result)
    {
        ml.cbStruct        = sizeof(MIXERLINE);
        ml.dwComponentType = MIXERLINE_COMPONENTTYPE_DST_SPEAKERS;

        // get the speaker line of the mixer device
        result = mixerGetLineInfo((HMIXEROBJ) hMixer, &ml, MIXER_GETLINEINFOF_COMPONENTTYPE);
        if (MMSYSERR_NOERROR == result)
        {
            mlc.cbStruct      = sizeof(MIXERLINECONTROLS);
            mlc.dwLineID      = ml.dwLineID;
            mlc.dwControlType = MIXERCONTROL_CONTROLTYPE_VOLUME;
            mlc.cControls     = 1;
            mlc.pamxctrl      = &mc;
            mlc.cbmxctrl      = sizeof(MIXERCONTROL);

            // get the volume controls associated with the speaker line
            result = mixerGetLineControls((HMIXEROBJ) hMixer, &mlc, MIXER_GETLINECONTROLSF_ONEBYTYPE);
            if (MMSYSERR_NOERROR == result)
            {
                mcdu.dwValue    = dwVolume;

                mcd.cbStruct    = sizeof(MIXERCONTROLDETAILS);
                mcd.hwndOwner   = 0;
                mcd.dwControlID = mc.dwControlID;
                mcd.paDetails   = &mcdu;
                mcd.cbDetails   = sizeof(MIXERCONTROLDETAILS_UNSIGNED);
                mcd.cChannels   = 1;

                // set the volume
                result = mixerSetControlDetails((HMIXEROBJ) hMixer, &mcd, MIXER_SETCONTROLDETAILSF_VALUE);
                if (MMSYSERR_NOERROR == result)
                    AfxMessageBox("Volume changed!");
                else
                    AfxMessageBox("mixerSetControlDetails() failed");
            }
            else
                AfxMessageBox("mixerGetLineControls() failed");
        }
        else
            AfxMessageBox("mixerGetLineInfo() failed");
    
        mixerClose(hMixer);
    }
    else
        AfxMessageBox("mixerOpen() failed");
}

//====================================================================

DWORD CVolumeDlg::GetVolume( void )
{
    DWORD                           dwVolume = -1;
    MMRESULT                        result;
    HMIXER                          hMixer;
    MIXERLINE                       ml   = {0};
    MIXERLINECONTROLS               mlc  = {0};
    MIXERCONTROL                    mc   = {0};
    MIXERCONTROLDETAILS             mcd  = {0};
    MIXERCONTROLDETAILS_UNSIGNED    mcdu = {0};


    // get a handle to the mixer device
    result = mixerOpen(&hMixer, 0, 0, 0, MIXER_OBJECTF_HMIXER);
    if (MMSYSERR_NOERROR == result)
    {
        ml.cbStruct        = sizeof(MIXERLINE);
        ml.dwComponentType = MIXERLINE_COMPONENTTYPE_DST_SPEAKERS;

        // get the speaker line of the mixer device
        result = mixerGetLineInfo((HMIXEROBJ) hMixer, &ml, MIXER_GETLINEINFOF_COMPONENTTYPE);
        if (MMSYSERR_NOERROR == result)
        {
            mlc.cbStruct      = sizeof(MIXERLINECONTROLS);
            mlc.dwLineID      = ml.dwLineID;
            mlc.dwControlType = MIXERCONTROL_CONTROLTYPE_VOLUME;
            mlc.cControls     = 1;
            mlc.pamxctrl      = &mc;
            mlc.cbmxctrl      = sizeof(MIXERCONTROL);

            // get the volume controls associated with the speaker line
            result = mixerGetLineControls((HMIXEROBJ) hMixer, &mlc, MIXER_GETLINECONTROLSF_ONEBYTYPE);
            if (MMSYSERR_NOERROR == result)
            {
                mcd.cbStruct    = sizeof(MIXERCONTROLDETAILS);
                mcd.hwndOwner   = 0;
                mcd.dwControlID = mc.dwControlID;
                mcd.paDetails   = &mcdu;
                mcd.cbDetails   = sizeof(MIXERCONTROLDETAILS_UNSIGNED);
                mcd.cChannels   = 1;

                // get the volume
                result = mixerGetControlDetails((HMIXEROBJ) hMixer, &mcd, MIXER_SETCONTROLDETAILSF_VALUE);
                if (MMSYSERR_NOERROR == result)
                    dwVolume = mcdu.dwValue;
                else
                    AfxMessageBox("mixerGetControlDetails() failed");
            }
            else
                AfxMessageBox("mixerGetLineControls() failed");
        }
        else
            AfxMessageBox("mixerGetLineInfo() failed");
    
        mixerClose(hMixer);
    }
    else
        AfxMessageBox("mixerOpen() failed");

    return (dwVolume);
}

//====================================================================

void CVolumeDlg::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar) 
{
    CString strVolume;


    strVolume.Format("%d", m_sliderVolume.GetPos());
    m_ctlVolume.SetWindowText(strVolume);

	CDialog::OnHScroll(nSBCode, nPos, pScrollBar);

    m_btnOK.EnableWindow(TRUE);
}

//====================================================================

void CVolumeDlg::OnOK() 
{
    DWORD   dwVolume;


    dwVolume = m_sliderVolume.GetPos();
    SetVolume(dwVolume);
	
	CDialog::OnOK();
}
