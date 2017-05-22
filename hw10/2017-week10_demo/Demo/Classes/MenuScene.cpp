#include "MenuScene.h"
#include "GameScene.h"
USING_NS_CC;

Scene* MenuScene::createScene()
{
    // 'Scene' is an autorelease object
    auto Scene = Scene::create();

    // 'layer' is an autorelease object
    auto layer = MenuScene::create();

    // add layer as a child to Scene
    Scene->addChild(layer);

    // return the Scene
    return Scene;
}

// on "init" you need to initialize your instance
bool MenuScene::init()
{

    if ( !Layer::init() )
    {
        return false;
    }

    Size visibleSize = Director::getInstance()->getVisibleSize();
    Vec2 origin = Director::getInstance()->getVisibleOrigin();

	auto gold_miner = Sprite::create("gold-miner-text.png");
	gold_miner->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y + 200));
	this->addChild(gold_miner, 1);

	auto bg_sky = Sprite::create("menu-background-sky.jpg");
	bg_sky->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y + 150));
	this->addChild(bg_sky, 0);

	auto bg = Sprite::create("menu-background.png");
	bg->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y - 60));
	this->addChild(bg, 0);

	auto miner = Sprite::create("menu-miner.png");
	miner->setPosition(Vec2(150 + origin.x, visibleSize.height / 2 + origin.y - 60));
	this->addChild(miner, 1);

	auto gold = Sprite::create("menu-start-gold.png");
	gold->setPosition(Vec2(origin.x + visibleSize.width - gold->getContentSize().width / 2 - 40,
		origin.y + gold->getContentSize().height / 2 + 20));
	this->addChild(gold, 1);

	auto leg = Sprite::createWithSpriteFrameName("miner-leg-0.png");
	Animate* legAnimate = Animate::create(AnimationCache::getInstance()->getAnimation("legAnimation"));
	leg->runAction(RepeatForever::create(legAnimate));
	leg->setPosition(110 + origin.x, origin.y + 102);
	this->addChild(leg, 1);

	auto startItem = MenuItemImage::create(
		"start-0.png",
		"start-1.png",
		CC_CALLBACK_1(MenuScene::startMenuCallback, this));

	startItem->setPosition(Vec2(origin.x + visibleSize.width - startItem->getContentSize().width / 2 - 85,
		origin.y + startItem->getContentSize().height / 2 + 165));

	// create menu, it's an autorelease object
	auto menu = Menu::create(startItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, 1);

    return true;
}

void MenuScene::startMenuCallback(Ref* pSender)
{
	auto gameScene = GameScene::createScene();
	Director::getInstance()->replaceScene(TransitionFade::create(0.5, gameScene, Color3B(0, 255, 255)));
	Director::getInstance()->replaceScene(TransitionFlipX::create(2, gameScene));
	Director::getInstance()->replaceScene(TransitionSlideInT::create(1, gameScene));
}