#include"Monster.h"
USING_NS_CC;

Factory* Factory::factory = NULL;
Factory::Factory() 
{
	initSpriteFrame();
}
Factory* Factory::getInstance() 
{
	if (factory == NULL) 
	{
		factory = new Factory();
	}
	return factory;
}
void Factory::initSpriteFrame()
{
	auto texture = Director::getInstance()->getTextureCache()->addImage("Monster.png");
	monsterDead.reserve(4);
	for (int i = 0; i < 4; i++) 
	{
		auto frame = SpriteFrame::createWithTexture(texture, CC_RECT_PIXELS_TO_POINTS(Rect(258-48*i,0,42,42)));
		monsterDead.pushBack(frame);
	}
}

Sprite* Factory::createMonster() 
{
	Sprite* mons = Sprite::create("Monster.png", CC_RECT_PIXELS_TO_POINTS(Rect(364,0,42,42)));
	monster.pushBack(mons);
	return mons;
}

void Factory::removeMonster(Sprite* sp) 
{
	//erase()执行后会返回指向下一个元素的迭代器
	auto animation = Animation::createWithSpriteFrames(monsterDead, 0.025f);
	auto animate = Animate::create(animation);
	Sequence* seq = Sequence::create(animate, CallFunc::create(CC_CALLBACK_0(Sprite::removeFromParent, sp)), NULL);
	sp->runAction(seq);
	monster.eraseObject(sp);
}
void Factory::moveMonster(Vec2 playerPos,float time)
{
	cocos2d:Vector<Sprite*>::iterator it = monster.begin();
	for (; it != monster.end();)
	{
		Vec2 monsterPos = (*it)->getPosition();
		Vec2 direction = playerPos - monsterPos;
		direction.normalize();
		(*it)->runAction(MoveBy::create(time, direction * 30));

		it++;
	}
}

Sprite* Factory::collider(Rect rect) 
{
	cocos2d:Vector<Sprite*>::iterator it = monster.begin();
	for (; it != monster.end();)
	{
		if (rect.containsPoint((*it)->getPosition()))
		{
			//erase()执行后会返回指向下一个元素的迭代器 
			return (*it);
		}
		else
		{
			it++;
			//do something 
		}
	}
}