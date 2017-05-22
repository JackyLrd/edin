#include "GameScene.h"
#include <random>

USING_NS_CC;

Scene* GameScene::createScene()
{
	// 'Scene' is an autorelease object
	auto Scene = Scene::create();

	// 'layer' is an autorelease object
	auto layer = GameScene::create();

	// add layer as a child to Scene
	Scene->addChild(layer);

	// return the Scene
	return Scene;
}

// on "init" you need to initialize your instance
bool GameScene::init()
{

	if (!Layer::init())
	{
		return false;
	}

	//add touch listener
	EventListenerTouchOneByOne* listener = EventListenerTouchOneByOne::create();
	listener->setSwallowTouches(true);
	listener->onTouchBegan = CC_CALLBACK_2(GameScene::onTouchBegan, this);
	Director::getInstance()->getEventDispatcher()->addEventListenerWithSceneGraphPriority(listener, this);


	Size visibleSize = Director::getInstance()->getVisibleSize();
	Vec2 origin = Director::getInstance()->getVisibleOrigin();

	auto bg = Sprite::create("level-background-0.jpg");
	bg->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y));
	this->addChild(bg, 0);

	auto shootItem = MenuItemImage::create(
		"menu-shoot.png",
		"menu-shoot.png",
		CC_CALLBACK_1(GameScene::shootMenuCallback, this));

	shootItem->setPosition(Vec2(visibleSize.width / 2 + origin.x + 750, visibleSize.height / 2 + origin.y + 100));

	// create menu, it's an autorelease object
	auto menu = Menu::create(shootItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, 1);

	stoneLayer = Layer::create();
	stone = Sprite::create("stone.png");
	stone->setPosition(Vec2(560, 480));
	stoneLayer->addChild(stone, 0);
	this->addChild(stoneLayer, 0);

	auto frameCache = SpriteFrameCache::getInstance();
	frameCache->addSpriteFramesWithFile("level-sheet.plist");
	mouse = Sprite::createWithSpriteFrameName("gem-mouse-0.png");
	mouse->setPosition(Vec2(visibleSize.width / 2 + origin.x, 0));
	Vector<SpriteFrame*> vec;
	char name[30] = { 0 };
	for (int i = 0; i<= 7; i++) {
		sprintf(name, "gem-mouse-%d.png", i);
		vec.pushBack(frameCache->getSpriteFrameByName(name));
	}
	auto animation = Animation::createWithSpriteFrames(vec);
	animation->setDelayPerUnit(0.1f);
	animation->setLoops(-1);
	animation->setRestoreOriginalFrame(true);
	auto animate = Animate::create(animation);
	mouse->runAction(animate);
	mouseLayer = Layer::create();
	mouseLayer->addChild(mouse, 0);
	mouseLayer->setPosition(Vec2(0, visibleSize.height / 2 + origin.y));
	this->addChild(mouseLayer, 0);

	return true;
}

bool GameScene::onTouchBegan(Touch *touch, Event *unused_event) {

	auto location = touch->getLocation();
	CCPoint point = mouseLayer->convertToNodeSpace(location);

	auto cheese = Sprite::create("cheese.png");
	auto fadeOut = FadeOut::create(2.0f);
	cheese->runAction(fadeOut);
	cheese->setPosition(location);
	this->addChild(cheese, 0);

	auto moveTo = MoveTo::create(1, point);
	mouse->runAction(moveTo);

	return true;
}

void GameScene::menuCloseCallback(Ref* pSender)
{
	Director::getInstance()->end();

#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
	exit(0);
#endif
}

void GameScene::shootMenuCallback(Ref* pSender)
{
	CCPoint location = mouseLayer->convertToWorldSpace(mouse->getPosition());

	auto stone2 = Sprite::create("stone.png");
	stone2->setPosition(Vec2(560, 480));
	auto moveTo = MoveTo::create(1, location);
	auto fadeOut = FadeOut::create(2.0f);
	auto seq = Sequence::create(moveTo, fadeOut, nullptr);
	stone2->runAction(seq);
	this->addChild(stone2, 0);

	Size visibleSize = Director::getInstance()->getVisibleSize();
	int w = location.x;
	int h = location.y;
	while (w <= location.x + 25 && w >= location.x - 25)
	{
		w = getRand(0, visibleSize.width);
	}
	while (h <= location.y + 25 && h >= location.y - 25)
	{
		h = getRand(0, visibleSize.height);
	}
	auto moveTo2 = MoveTo::create(1, Vec2(w, h));
	mouse->runAction(moveTo2);

	auto frameCache = SpriteFrameCache::getInstance();
	frameCache->addSpriteFramesWithFile("level-sheet.plist");
	auto diamond = Sprite::createWithSpriteFrameName("diamond-0.png");
	diamond->setPosition(location);
	Vector<SpriteFrame*> vec;
	char name[20];
	for (int i = 0; i <= 6; i++) {
		sprintf(name, "diamond-%d.png", i);
		vec.pushBack(frameCache->getSpriteFrameByName(name));
	}
	auto animation = Animation::createWithSpriteFrames(vec);
	animation->setDelayPerUnit(0.1f);
	animation->setLoops(-1);
	animation->setRestoreOriginalFrame(true);
	auto animate = Animate::create(animation);
	diamond->runAction(animate);
	this->addChild(diamond, 0);
}

int GameScene::getRand(int start, int end)
{
	float i = CCRANDOM_0_1()*(end - start + 1) + start;  //产生一个从start到end间的随机数
	return (int)i;
}