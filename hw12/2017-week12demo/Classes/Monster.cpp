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
	cocos2d:Vector<Sprite*>::iterator it = monster.begin(); 
	for (; it != monster.end();) 
	{
		if (sp == (*it)) 
		{ 
			//erase()执行后会返回指向下一个元素的迭代器
			auto animation = Animation::createWithSpriteFrames(monsterDead, 0.025f);
			auto animate = Animate::create(animation);
			(*it)->runAction(animate);
			it = monster.erase(it); 
		} 
		else
		{ 
			it++; 
			//do something 
		} 
	}
}
void Factory::moveMonster(Vec2 playerPos,float time)
{
	auto moveTo = MoveTo::create(time, playerPos);

	cocos2d:Vector<Sprite*>::iterator it = monster.begin();
	for (; it != monster.end();)
	{
		it++;
		//do something
		(*it)->runAction(moveTo);
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
			removeMonster(*it);
		}
		else
		{
			it++;
			//do something 
		}
	}
}