---GamePad�̓������@---

1. �V�[�����GamePad�I�u�W�F�N�g��z�u���܂��B
���V�[�����EventSystem�I�u�W�F�N�g�����݂���ƃG���[���o��\��������ׁAActive��false�ɂ��邩�폜���Ă��������B

2. GamePad�̃C���X�y�N�^�[����GamePadControl��UsePs4Controller��true�ɂ����PS4�̃R���g���[���[���͂����m�ł���悤�ɂȂ�܂��B�i�f�t�H���g��true�j

3. AxisValue�̒l��ς���ƁA�R���g���[���[�̃X�e�B�b�N�̗L�����͊��x�𒲐߂ł��܂��B 


---�g����---

1. �R���g���[���[�̃{�^�����͂����m����B

�@�@�@�@GamePadControl.Instance.GetButtonDown(ControllerNum.P1, DS4ButtonKey.Circle)    1P�R���g���[���[�́Z�{�^���������ꂽ���Ƃ�bool�^�Ŏ擾����i�����ꂽ�ꍇ��true�j

        ---DS4ButtonKey�ꗗ---
		Circle      �Z�{�^��
		Cross       �~�{�^��
		Triangle    ���{�^��
        Square      ���{�^��
        Up          �\���L�[ ��
        Down        �\���L�[ ��
        Left        �\���L�[ ��
        Right       �\���L�[ ��
        L1�@�@�@�@�@L1�{�^��
        L3�@�@�@�@�@L�X�e�B�b�N��������
        R1          R1�{�^��
        R3          R�X�e�B�b�N��������
        OPTION      Option�{�^��
        SHARE       Share�{�^��


		�g�p�� :

		if(GamePadControl.Instance.GetButtonDown(ControllerNum.P1, DS4ButtonKey.Up) == true)
		{
		    // 1P�R���g���[���[�̏\���L�[���������ꂽ����s����������

		}



2. �R���g���[���[�̃X�e�B�b�N���͂����m����B

�@�@�@�@GamePadControl.Instance.GetAxisDown(ControllerNum.P1, DS4AxisKey.L2)    1P�R���g���[���[��L2�{�^�������͂��ꂽ���Ƃ�bool�^�Ŏ擾����i�����ꂽ�ꍇ��true�j
        
		���͊��x��AxisValue�Œ��߉\

        ---DS4AxisKey�ꗗ---
		LeftStick_Up        ���X�e�B�b�N ��
		LeftStick_Down      ���X�e�B�b�N ��
		LeftStick_Left      ���X�e�B�b�N ��
        LeftStick_Right     ���X�e�B�b�N ��
        RightStick_Up       �E�X�e�B�b�N ��
        RightStick_Down     �E�X�e�B�b�N ��
        RightStick_Left     �E�X�e�B�b�N ��
        RightStick_Right    �E�X�e�B�b�N ��
        L2�@�@�@�@�@        L2�{�^��
        R2�@�@�@�@�@        R2�{�^��



3. StandaloneInputModule(UI����p�̃��W���[��)���g���B
        
		StandaloneInputModule���g���ꍇ�A�R���g���[����\�ߗL���ڑ����Ă����K�v������̂Œ��ӁB

		GamePadControl.Instance.SetInputModule(ControllerNum.P1)    1P�̃R���g���[���[��uGI�̃{�^�����͓��̑��삪�\�ɂȂ�܂��B