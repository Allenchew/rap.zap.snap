---GamePad�̓������@---

1. �V�[�����GamePad�I�u�W�F�N�g��z�u���܂��B
���V�[�����EventSystem�I�u�W�F�N�g�����݂���ƃG���[���o��\��������ׁAActive��false�ɂ��邩�폜���Ă��������B

2. GamePad�̃C���X�y�N�^�[����GamePadControl��UsePs4Controller��true�ɂ����PS4�̃R���g���[���[���͂����m�ł���悤�ɂȂ�܂��B�i�f�t�H���g��true�j

3. AxisValue�̒l��ς���ƁA�R���g���[���[�̃X�e�B�b�N�̗L�����͊��x�𒲐߂ł��܂��B 


---�g����---

1. �R���g���[���[�̓��͂����m����B

�@�@�@�@GamePadControl.Instance.Controller1.Circle    1P�R���g���[���[�́Z�{�^���������ꂽ���Ƃ�bool�^�Ŏ擾����i�����ꂽ�ꍇ��true�j

�@�@�@�@GamePadControl.Instance.Controller2.Cross     2P�R���g���[���[�́~�{�^���������ꂽ���Ƃ�bool�^�Ŏ擾����

        ---�擾�ł���L�[---
		Circle      �Z�{�^��
		Cross       �~�{�^��
		Triangle    ���{�^��
        Square      ���{�^��
        Up          �\���L�[ ��
        Down        �\���L�[ ��
        Left        �\���L�[ ��
        Right       �\���L�[ ��
        L1�@�@�@�@�@L1�{�^��
        L2�@�@�@�@�@L2�{�^��
        L3�@�@�@�@�@L�X�e�B�b�N��������
        LstickU     L�X�e�B�b�N ��
        LstickD     L�X�e�B�b�N ��
        LstickL     L�X�e�B�b�N ��
        LstickR     L�X�e�B�b�N ��
        R1          R1�{�^��
        R2          R2�{�^��
        R3          R�X�e�B�b�N��������
        RstickU     R�X�e�B�b�N ��
        RstickD     R�X�e�B�b�N ��
        RstickL     R�X�e�B�b�N ��
        RstickR     R�X�e�B�b�N ��
        OPTION      Option�{�^��
        SHARE       Share�{�^��


		�g�p�� :

		if(GamePadControl.Instance.Controller1.Upley)
		{
		    // 1P�R���g���[���[�̏\���L�[���������ꂽ����s����������

		}

2. StandaloneInputModule(UI����p�̃��W���[��)���g���B
        
		StandaloneInputModule���g���ꍇ�A�R���g���[����\�ߗL���ڑ����Ă����K�v������̂Œ��ӁB

		GamePadControl.Instance.SetInputModule(���͑ΏۃR���g���[���[)    �w�肵���R���g���[���[��uGI�̃{�^�����͓��̑��삪�\�ɂȂ�܂��B