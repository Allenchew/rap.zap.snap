2019/11/19 �X�V

---NotesObject�������@---
1. �V�[�����NotesObject��z�u����

2. �C���X�y�N�^�[�Őݒ�\�ȍ���
    
	NotesUIMode    true:  �m�[�c��UI�Ƃ��Đ���
	               false: �m�[�c��Sprite�Ƃ��Đ���

    NotesSize    �@       ���������m�[�c��Scale

	NotesSpriteAlpha      ����m�[�c�̓����x

	PerfectLength         �m�[�c��Perfect�����i�l���������قǔ��肪�V�r�A�ɂȂ�܂��j

	GoodLength            �m�[�c��Good�����i�l���������قǔ��肪�V�r�A�ɂȂ�܂��j

	BadLength             �m�[�c��Bad�����i�l���������قǔ��肪�V�r�A�ɂȂ�܂��j


---�g����---
1. �X�N���v�g����Ăяo���ꍇ�ANotesControl.Instance.�Ăяo�������֐��� �ŌĂяo���܂�

2. �Ăяo����֐��ꗗ
    
	PlayNotesOneShot()        �m�[�c���w�肵�����W�ɂP��Đ�����
	�@�@�@�@�@�@�@�@�@�@�@�@�@�� ������GameObject�ɂ��邱�ƂŁA���̃I�u�W�F�N�g�̍��W�Ƀm�[�c���Đ����邱�Ƃ��\


	PlayNotes()               �m�[�c���w�肵�����W�Ɏw�肵���񐔂����Đ�����
	                          �� NotesControl.Instance.PlayNotes(new Vector3(-5, -5, 0), new Vector3(5, 5, 0), InputController.PlayerOne, 1.5f, 15, 1.0f)
							     �m�[�c��(-5, -5, 0)����(5, 5, 0)�܂ł�1.5�b�Ԃňړ����鏈����1�b���ƂōĐ����A15��J��Ԃ�����
							  �� ������Vector3[]��AGameObject[]��錾���邱�Ƃ��\
							     GameObject[]�Ƃ��Đ錾����ꍇ�A�V�[����ɓK����GameObject��z�u���Ă����z��f�[�^�Ɋi�[���Ă����ƁA���̔z��f�[�^
								 ��錾�����ۂɂ���GameObject�̍��W�Ƀm�[�c���Đ������悤�ɂ��邱�Ƃ��ł���


    GetResult()              �m�[�c�̔��茋�ʂ��擾�ł���
	                         �� GetResult(0, InputController.PlayerOne) 1P��Bad�̐����擾����


	ResetResult()            �m�[�c�̔��茋�ʂ����Z�b�g����
	                         �� ResetResult(InputController.PlayerTwo) 2P�̔��茋�ʂ����Z�b�g����



---�m�[�c�̓��́i�e�X�g�v���C�p�j---

�Z�{�^�� == A�L�[
�~�{�^�� == S�L�[
���{�^�� == D�L�[
�\���L�[ == ���L�[
							  �@ 