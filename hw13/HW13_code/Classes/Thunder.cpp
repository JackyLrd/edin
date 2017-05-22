﻿#include "Thunder.h"
#include <algorithm>
#include "SimpleAudioEngine.h"

USING_NS_CC;

using namespace CocosDenshion;

Scene* Thunder::createScene() 
{
	// 'scene' is an autorelease object
	auto scene = Scene::create();

	// 'layer' is an autorelease object
	auto layer = Thunder::create();

	// add layer as a child to scene
	scene->addChild(layer);

	// return the scene
	return scene;
}

bool Thunder::init() 
{
	if (!Layer::init()) 
	{
		return false;
	}

	isMove = false;  // 是否点击飞船
	visibleSize = Director::getInstance()->getVisibleSize();

	// 创建背景
	auto bgsprite = Sprite::create("bg.jpg");
	bgsprite->setPosition(visibleSize / 2);
	bgsprite->setScale(visibleSize.width / bgsprite->getContentSize().width,
						 visibleSize.height / bgsprite->getContentSize().height);
	this->addChild(bgsprite, 0);

	// 创建飞船
	player = Sprite::create("player.png");
	player->setAnchorPoint(Vec2(0.5, 0.5));
	player->setPosition(visibleSize.width / 2, player->getContentSize().height);
	player->setName("player");
	this->addChild(player, 1);

	// 显示陨石和子弹数量
	enemysNum = Label::createWithTTF("enemys: 0", "fonts/arial.TTF", 20);
	enemysNum->setColor(Color3B(255, 255, 255));
	enemysNum->setPosition(50, 60);
	this->addChild(enemysNum, 3);
	bulletsNum = Label::createWithTTF("bullets: 0", "fonts/arial.TTF", 20);
	bulletsNum->setColor(Color3B(255, 255, 255));
	bulletsNum->setPosition(50, 30);
	this->addChild(bulletsNum, 3);

	addEnemy(5);        // 初始化陨石
	flag = 1;
	preloadMusic();     // 预加载音乐
	playBgm();          // 播放背景音乐
	explosion();        // 创建爆炸帧动画

	// 添加监听器
	addTouchListener();
	addKeyboardListener();
	addCustomListener();

	// 调度器
	schedule(schedule_selector(Thunder::update), 0.04f, kRepeatForever, 0);

	return true;
}

//预加载音乐文件
void Thunder::preloadMusic() 
{
	// Todo
	auto audio = SimpleAudioEngine::getInstance();
	audio->preloadBackgroundMusic("music/bgm.mp3");
	audio->preloadEffect("music/explode.wav");
	audio->preloadEffect("music/fire.wav");
}

//播放背景音乐
void Thunder::playBgm() 
{
	// Todo
	auto audio = SimpleAudioEngine::getInstance();
	//audio->playBackgroundMusic("music/bgm.mp3", true);
}

//初始化陨石
void Thunder::addEnemy(int n) 
{
	enemys.clear();
	for (unsigned i = 0; i < 3; ++i) 
	{
		char enemyPath[20];
		sprintf(enemyPath, "stone%d.png", 3 - i);
		double width = visibleSize.width / (5 + 1.0),
			   height = visibleSize.height - (50 * (i + 1));
		for (int j = 0; j < 5; ++j) 
		{
		  auto enemy = Sprite::create(enemyPath);
		  enemy->setAnchorPoint(Vec2(0.5, 0.5));
		  enemy->setScale(0.5, 0.5);
		  enemy->setPosition(width * (j + 1), height);
		  enemys.push_back(enemy);
		  addChild(enemy, 1);
		}
	}
}

// 陨石向下移动并生成新的一行(加分项)
void Thunder::newEnemy() 
{
	// Todo
	list<Sprite*>::iterator it = enemys.begin();
	while (it != enemys.end())
	{
		(*it)->setPosition(Vec2((*it)->getPosition().x, (*it)->getPosition().y - 50));
		it++;
	}

	for (unsigned i = flag; flag < i + 1; flag++)
	{
		char enemyPath[20];
		sprintf(enemyPath, "stone%d.png", i);
		double width = visibleSize.width / (5 + 1.0),
			height = visibleSize.height - 50;
		for (int j = 0; j < 5; ++j)
		{
			auto enemy = Sprite::create(enemyPath);
			enemy->setAnchorPoint(Vec2(0.5, 0.5));
			enemy->setScale(0.5, 0.5);
			enemy->setPosition(width * (j) + 75, height);
			enemys.push_back(enemy);
			addChild(enemy, 1);
		}
	}
	if (flag == 4) flag = 1;
}

// 移动飞船
void Thunder::movePlane(char c) 
{
	// Todo
	if (c == 'A')
	{
		if (player->getPosition().x - 30 < 30)
		{
			player->runAction(MoveBy::create(0.1f, Vec2(-player->getPosition().x + 30, 0)));
		}
		else
			player->runAction(MoveBy::create(0.1f, Vec2(-30, 0)));
	}
	else
	{
		if (player->getPosition().x + 30 > visibleSize.width - 30)
		{
			player->runAction(MoveBy::create(0.1f, Vec2(visibleSize.width - player->getPosition().x - 30, 0)));
		}
		else
			player->runAction(MoveBy::create(0.1f, Vec2(30, 0)));
	}
}

//发射子弹
void Thunder::fire() 
{
	auto bullet = Sprite::create("bullet.png");
	bullet->setAnchorPoint(Vec2(0.5, 0.5));
	bullets.push_back(bullet);
	bullet->setPosition(player->getPosition());
	addChild(bullet, 1);
	SimpleAudioEngine::getInstance()->playEffect("music/fire.wav", false);

	// 移除飞出屏幕外的子弹
	// Todo

	bullet->runAction(MoveBy::create(1.0f, Vec2(0, visibleSize.height)));
}

// 切割爆炸动画帧
void Thunder::explosion() 
{
	// Todo
	auto texture = Director::getInstance()->getTextureCache()->addImage("explosion.png");
	explode.reserve(9);
	for (int i = 0; i < 9; i++) 
	{
		auto frame = SpriteFrame::createWithTexture(texture, CC_RECT_PIXELS_TO_POINTS(Rect(188.8 * (i % 5) + 2, i / 5 * 192, 188.8, 192)));
		explode.pushBack(frame);
	}
}

void Thunder::update(float f) 
{
	// 实时更新页面内陨石和子弹数量(不得删除)
	// 要求数量显示正确(加分项)
	char str[15];
	sprintf(str, "enemys: %d", enemys.size());
	enemysNum->setString(str);
	sprintf(str, "bullets: %d", bullets.size());
	bulletsNum->setString(str);

	// 飞船移动
	if (isMove)
		this->movePlane(movekey);

	static int ct = 0;
	static int dir = 4;
	++ct;
	if (ct == 120)
		ct = 40, dir = -dir;
	else if (ct == 80) 
	{
		dir = -dir;
		newEnemy();  
		// 陨石向下移动并生成新的一行(加分项)
	}
	else if (ct == 20)
		ct = 40, dir = -dir;

	//陨石左右移动
	for (Sprite* s : enemys) 
	{
		if (s != NULL) 
		{
		  s->setPosition(s->getPosition() + Vec2(dir, 0));
		}
	}

	// 分发自定义事件
	EventCustom e("meet");
	_eventDispatcher->dispatchEvent(&e);
}

// 自定义碰撞事件
void Thunder::meet(EventCustom * event) 
{
	// 判断子弹是否打中陨石并执行对应操作
	// Todo

	list<Sprite*>::iterator it = bullets.end();
	for (; it != bullets.begin();)
	{
		it--;
		list<Sprite*>::iterator m_it = enemys.end();
		for (; m_it != enemys.begin();)
		{
			m_it--;
			if ((*m_it)->getPosition().getDistance((*it)->getPosition()) < 25)
			{
				SimpleAudioEngine::getInstance()->playEffect("music/explode.wav", false);

				Sprite* temp = (*m_it);
				temp->runAction(Sequence::create(Animate::create(Animation::createWithSpriteFrames(explode, 0.05f, 1)), CallFunc::create([temp] {temp->removeFromParentAndCleanup(true); }), nullptr));
				enemys.erase(m_it);

				Sprite* temp2 = (*it);
				temp2->runAction(Sequence::create(CallFunc::create([temp2] {temp2->removeFromParentAndCleanup(true); }), nullptr));
				it = bullets.erase(it);
				break;
			}
		}
	}

	// 判断游戏是否结束并执行对应操作
	// Todo
	list<Sprite*>::iterator e_it = enemys.end();
	for (; e_it != enemys.begin();)
	{
		e_it--;
		if ((*e_it)->getPosition().y < player->getPosition().y + 50)
		{
			SimpleAudioEngine::getInstance()->playEffect("music/explode.wav", false);
			Sprite* temp = player;
			temp->runAction(Sequence::create(Animate::create(Animation::createWithSpriteFrames(explode, 0.05f, 1)), CallFunc::create([temp] {temp->removeFromParentAndCleanup(true); }), nullptr));
			_eventDispatcher->removeAllEventListeners();

			auto sprite = Sprite::create("gameOver.png");
			sprite->setPosition(visibleSize / 2);
			this->addChild(sprite, 0);

			this->unscheduleAllSelectors();

			return;
		}
	}

	list<Sprite*>::iterator b_it = bullets.end();
	for (; b_it != bullets.begin();)
	{
		b_it--;
		if ((*b_it)->getPosition().x > visibleSize.width || (*b_it)->getPosition().x < 0 || (*b_it)->getPosition().y > visibleSize.height || (*b_it)->getPosition().y < 0)
		{
			(*b_it)->removeFromParentAndCleanup(true);
			b_it = bullets.erase(b_it);
		}
	}
}

// 添加自定义监听器
void Thunder::addCustomListener() 
{
	// Todo
	auto meetListenr = EventListenerCustom::create("meet", CC_CALLBACK_1(Thunder::meet, this));
	_eventDispatcher->addEventListenerWithFixedPriority(meetListenr, 1);
}

// 添加键盘事件监听器
void Thunder::addKeyboardListener() 
{
	// Todo
	auto keyboardListener = EventListenerKeyboard::create();
	keyboardListener->onKeyPressed = CC_CALLBACK_2(Thunder::onKeyPressed, this);
	keyboardListener->onKeyReleased = CC_CALLBACK_2(Thunder::onKeyReleased, this);
	_eventDispatcher->addEventListenerWithFixedPriority(keyboardListener, 1);
}

void Thunder::onKeyPressed(EventKeyboard::KeyCode code, Event* event) 
{
	switch (code) 
	{
		case EventKeyboard::KeyCode::KEY_LEFT_ARROW:
		case EventKeyboard::KeyCode::KEY_CAPITAL_A:
		case EventKeyboard::KeyCode::KEY_A:
		movekey = 'A';
		isMove = true;
		break;
		case EventKeyboard::KeyCode::KEY_RIGHT_ARROW:
		case EventKeyboard::KeyCode::KEY_CAPITAL_D:
		case EventKeyboard::KeyCode::KEY_D:
		movekey = 'D';
		isMove = true;
		break;
		case EventKeyboard::KeyCode::KEY_SPACE:
		fire();
		break;
	}
}

void Thunder::onKeyReleased(EventKeyboard::KeyCode code, Event* event) 
{
	switch (code) 
	{
		case EventKeyboard::KeyCode::KEY_LEFT_ARROW:
		case EventKeyboard::KeyCode::KEY_A:
		case EventKeyboard::KeyCode::KEY_CAPITAL_A:
		case EventKeyboard::KeyCode::KEY_RIGHT_ARROW:
		case EventKeyboard::KeyCode::KEY_D:
		case EventKeyboard::KeyCode::KEY_CAPITAL_D:
		isMove = false;
		break;
	}
}

// 添加触摸事件监听器
void Thunder::addTouchListener() 
{
	// Todo
	auto touchListener = EventListenerTouchOneByOne::create();
	touchListener->onTouchBegan = CC_CALLBACK_2(Thunder::onTouchBegan, this); 
	touchListener->onTouchEnded = CC_CALLBACK_2(Thunder::onTouchEnded, this);
	touchListener->onTouchMoved = CC_CALLBACK_2(Thunder::onTouchMoved, this);
	_eventDispatcher->addEventListenerWithFixedPriority(touchListener, 1);
}

// 鼠标点击发射炮弹
bool Thunder::onTouchBegan(Touch *touch, Event *event) 
{
	fire();
	if (player->getBoundingBox().containsPoint(touch->getLocation()))
	{
		isClick = true;
	}
	return true;
}

void Thunder::onTouchEnded(Touch *touch, Event *event) 
{
	isClick = false;
}

// 当鼠标按住飞船后可控制飞船移动 (加分项)
void Thunder::onTouchMoved(Touch *touch, Event *event) 
{
	// Todo
	Vec2 delta = touch->getDelta();
	if(isClick)
		player->setPosition(Vec2(player->getPosition().x + delta.x, player->getPosition().y));
}