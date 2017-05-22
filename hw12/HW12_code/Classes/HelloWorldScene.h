#ifndef __HELLOWORLD_SCENE_H__
#define __HELLOWORLD_SCENE_H__

#include "cocos2d.h"
#include "sqlite3\include\sqlite3.h"
using namespace cocos2d;
class HelloWorld : public cocos2d::Layer
{
public:
    static cocos2d::Scene* createScene();

    virtual bool init();
    
    // implement the "static create()" method manually
    CREATE_FUNC(HelloWorld);
	//void update(float dt) override;
	void W_menuCloseCallback(Ref* pSender);
	void A_menuCloseCallback(Ref* pSender);
	void S_menuCloseCallback(Ref* pSender);
	void D_menuCloseCallback(Ref* pSender);
	void X_menuCloseCallback(Ref* pSender);
	void Y_menuCloseCallback(Ref* pSender);
	void updateCustom(float dt);
	void hitByMonster(float dt);
	bool attackMonster();
private:
	cocos2d::Sprite* player;
	cocos2d::Vector<SpriteFrame*> attack;
	cocos2d::Vector<SpriteFrame*> dead;
	cocos2d::Vector<SpriteFrame*> run;
	cocos2d::Vector<SpriteFrame*> idle;
	cocos2d::Size visibleSize;
	cocos2d::Vec2 origin;
	cocos2d::Label* time;
	int dtime;
	cocos2d::ProgressTimer* pT;
	int killNum;
	bool turn;
	sqlite3* pdb;
};

#endif // __HELLOWORLD_SCENE_H__
