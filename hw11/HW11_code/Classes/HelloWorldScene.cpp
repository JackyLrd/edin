﻿#include "HelloWorldScene.h"
#pragma execution_character_set("utf-8")
USING_NS_CC;

Scene* HelloWorld::createScene()
{
    // 'scene' is an autorelease object
    auto scene = Scene::create();

    // 'layer' is an autorelease object
    auto layer = HelloWorld::create();

    // add layer as a child to scene
    scene->addChild(layer);

    // return the scene
    return scene;
}

// on "init" you need to initialize your instance
bool HelloWorld::init()
{
    //////////////////////////////
    // 1. super init first
	schedule(schedule_selector(HelloWorld::updateCustom), 1.0f, kRepeatForever, 0);
    if ( !Layer::init() )
    {
        return false;
    }

    visibleSize = Director::getInstance()->getVisibleSize();
    origin = Director::getInstance()->getVisibleOrigin();

	//创建一张贴图
	auto texture = Director::getInstance()->getTextureCache()->addImage("$lucia_2.png");
	//从贴图中以像素单位切割，创建关键帧
	auto frame0 = SpriteFrame::createWithTexture(texture, CC_RECT_PIXELS_TO_POINTS(Rect(0, 0, 113, 113)));
	//使用第一帧创建精灵
	player = Sprite::createWithSpriteFrame(frame0);
	player->setPosition(Vec2(origin.x + visibleSize.width / 2,
							origin.y + visibleSize.height/2));
	addChild(player, 3);

	//hp条
	Sprite* sp0 = Sprite::create("hp.png", CC_RECT_PIXELS_TO_POINTS(Rect(0, 320, 420, 47)));
	Sprite* sp = Sprite::create("hp.png", CC_RECT_PIXELS_TO_POINTS(Rect(610, 362, 4, 16)));

	//使用hp条设置progressBar
	pT = ProgressTimer::create(sp);
	pT->setScaleX(90);
	pT->setAnchorPoint(Vec2(0, 0));
	pT->setType(ProgressTimerType::BAR);
	pT->setBarChangeRate(Point(1, 0));
	pT->setMidpoint(Point(0, 1));
	pT->setPercentage(100);
	pT->setPosition(Vec2(origin.x+14*pT->getContentSize().width,origin.y + visibleSize.height - 2*pT->getContentSize().height));
	addChild(pT,1);
	sp0->setAnchorPoint(Vec2(0, 0));
	sp0->setPosition(Vec2(origin.x + pT->getContentSize().width, origin.y + visibleSize.height - sp0->getContentSize().height));
	addChild(sp0,0);

	// 静态动画
	idle.reserve(1);
	idle.pushBack(frame0);

	// 攻击动画
	attack.reserve(17);
	for (int i = 0; i < 17; i++) {
		auto frame = SpriteFrame::createWithTexture(texture, CC_RECT_PIXELS_TO_POINTS(Rect(113*i,0,113,113)));
		attack.pushBack(frame);
	}

	// 可以仿照攻击动画
	// 死亡动画(帧数：22帧，高：90，宽：79）
	auto texture2 = Director::getInstance()->getTextureCache()->addImage("$lucia_dead.png");
	dead.reserve(22);
	for (int i = 0; i < 22; i++) {
		auto frame = SpriteFrame::createWithTexture(texture2, CC_RECT_PIXELS_TO_POINTS(Rect(79 * i, 0, 79, 90)));
		dead.pushBack(frame);
	}

	// 运动动画(帧数：8帧，高：101，宽：68）
	auto texture3 = Director::getInstance()->getTextureCache()->addImage("$lucia_forward.png");
	run.reserve(8);
	for (int i = 0; i < 8; i++) {
		auto frame2 = SpriteFrame::createWithTexture(texture3, CC_RECT_PIXELS_TO_POINTS(Rect(68 * i, 0, 68, 101)));
		run.pushBack(frame2);
	}

	std::string ttfStr = "W";
	auto ttf_2 = Label::createWithTTF(ttfStr, "fonts/arial.ttf", 36);
	auto W_menuItem = MenuItemLabel::create(ttf_2, CC_CALLBACK_1(HelloWorld::W_menuCloseCallback, this));
	W_menuItem->setPosition(Vec2(origin.x + 60, origin.y + 80));

	ttfStr = "A";
	ttf_2 = Label::createWithTTF(ttfStr, "fonts/arial.ttf", 36);
	auto A_menuItem = MenuItemLabel::create(ttf_2, CC_CALLBACK_1(HelloWorld::A_menuCloseCallback, this));
	A_menuItem->setPosition(Vec2(origin.x + 25, origin.y + 40));

	ttfStr = "S";
	ttf_2 = Label::createWithTTF(ttfStr, "fonts/arial.ttf", 36);
	auto S_menuItem = MenuItemLabel::create(ttf_2, CC_CALLBACK_1(HelloWorld::S_menuCloseCallback, this));
	S_menuItem->setPosition(Vec2(origin.x + 60, origin.y + 40));

	ttfStr = "D";
	ttf_2 = Label::createWithTTF(ttfStr, "fonts/arial.ttf", 36);
	auto D_menuItem = MenuItemLabel::create(ttf_2, CC_CALLBACK_1(HelloWorld::D_menuCloseCallback, this));
	D_menuItem->setPosition(Vec2(origin.x + 95, origin.y + 40));

	ttfStr = "X";
	ttf_2 = Label::createWithTTF(ttfStr, "fonts/arial.ttf", 36);
	auto X_menuItem = MenuItemLabel::create(ttf_2, CC_CALLBACK_1(HelloWorld::X_menuCloseCallback, this));
	X_menuItem->setPosition(Vec2(origin.x + visibleSize.width - 25 , origin.y + 80));

	ttfStr = "Y";
	ttf_2 = Label::createWithTTF(ttfStr, "fonts/arial.ttf", 36);
	auto Y_menuItem = MenuItemLabel::create(ttf_2, CC_CALLBACK_1(HelloWorld::Y_menuCloseCallback, this));
	Y_menuItem->setPosition(Vec2(origin.x + visibleSize.width - 45, origin.y + 40));

	auto menu = Menu::create(W_menuItem, A_menuItem, S_menuItem, D_menuItem, X_menuItem, Y_menuItem, NULL);
	menu->setPosition(Vec2(origin.x, origin.y));
	this->addChild(menu, 0);

	dtime = 180;
	char str[10];
	sprintf(str, "%d", dtime);
	time = Label::createWithTTF(str, "fonts/arial.ttf", 36);
	time->setPosition(Vec2(origin.x + visibleSize.width / 2, origin.y + visibleSize.height - 50));
	this->addChild(time, 0);

	return true;
}

void HelloWorld::W_menuCloseCallback(Ref* pSender)
{
	auto animation = Animation::createWithSpriteFrames(run, 0.025f);
	auto animate = Animate::create(animation);

	auto location = player->getPosition();
	auto y = (location.y + 30 > origin.y + visibleSize.height - 65) ? origin.y + visibleSize.height - 65 : location.y + 30;
	auto moveTo = MoveTo::create(0.2, Vec2(location.x, y));

	player->runAction(animate); 
	player->runAction(moveTo);
}

void HelloWorld::A_menuCloseCallback(Ref* pSender)
{
	auto animation = Animation::createWithSpriteFrames(run, 0.025f);
	auto animate = Animate::create(animation);

	auto location = player->getPosition();
	auto x = (location.x - 30 < origin.x + 30) ? origin.x + 30 : location.x - 30;
	auto moveTo = MoveTo::create(0.2, Vec2(x, location.y));

	player->runAction(animate); 
	player->runAction(moveTo);
}

void HelloWorld::S_menuCloseCallback(Ref* pSender)
{
	auto animation = Animation::createWithSpriteFrames(run, 0.025f);
	auto animate = Animate::create(animation);

	auto location = player->getPosition();
	auto y = (location.y - 30 < origin.y + 30) ? origin.y + 30 : location.y - 30;
	auto moveTo = MoveTo::create(0.2, Vec2(location.x, y));

	player->runAction(animate);
	player->runAction(moveTo);
}

void HelloWorld::D_menuCloseCallback(Ref* pSender)
{
	auto animation = Animation::createWithSpriteFrames(run, 0.025f);
	auto animate = Animate::create(animation);

	auto location = player->getPosition();
	auto x = (location.x + 30 > origin.x + visibleSize.width - 30) ? origin.x + visibleSize.width - 30 : location.x + 30;
	auto moveTo = MoveTo::create(0.2, Vec2(x, location.y));

	player->runAction(animate);
	player->runAction(moveTo);
}

void HelloWorld::X_menuCloseCallback(Ref* pSender)
{
	player->stopAllActions();
	auto animation = Animation::createWithSpriteFrames(dead, 0.1f);
	auto animate = Animate::create(animation);

	auto animation2 = Animation::createWithSpriteFrames(idle, 0.1f);
	auto animate2 = Animate::create(animation2);

	auto action = Sequence::create(animate, animate2, NULL);

	player->runAction(action);
	
	auto pgto = ProgressTo::create(1.5f, pT->getPercentage() - 20);
	pT->runAction(pgto);
	pT->setPercentage(pT->getPercentage() - 20);
}

void HelloWorld::Y_menuCloseCallback(Ref* pSender)
{
	player->stopAllActions();
	auto animation = Animation::createWithSpriteFrames(attack, 0.1f);
	auto animate = Animate::create(animation);

	auto animation2 = Animation::createWithSpriteFrames(idle, 0.1f);
	auto animate2 = Animate::create(animation2);

	auto action = Sequence::create(animate, animate2, NULL);

	player->runAction(action);
	
	auto pgto = ProgressTo::create(1.5f, pT->getPercentage() + 20);
	pT->runAction(pgto);
	pT->setPercentage(pT->getPercentage() - 20);
}

void HelloWorld::updateCustom(float dt)
{
	if (time != NULL)
	{
		dtime -= 1;
		char str[10];
		sprintf(str, "%d", dtime);
		time->setString(str);
	}
}