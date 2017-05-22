#ifndef __GAME_Scene_H__
#define __GAME_Scene_H__

#include "cocos2d.h"
USING_NS_CC;

class GameScene : public cocos2d::Layer
{
public:
	static cocos2d::Scene* createScene();

	virtual bool init();
	int getRand(int start, int end);
	virtual bool onTouchBegan(Touch *touch, Event *unused_event);
	void menuCloseCallback(cocos2d::Ref* pSender);

	virtual void shootMenuCallback(Ref* pSender);

	CREATE_FUNC(GameScene);

private:
	Sprite* mouse;

	Sprite* stone;
	
	Layer* mouseLayer;
	
	Layer* stoneLayer;

};

#endif // __GAME_Scene_H__

