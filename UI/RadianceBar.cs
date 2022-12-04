using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System;
using Terraria.DataStructures;
using Terraria.GameContent;
using ReLogic.Graphics;
using excels.Items.Misc;

namespace excels.UI
{
    public class RadianceBar : UIState
	{// For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
	 // Once this is all set up make sure to go and do the required stuff for most UI's in the Mod class.
		private UIText text;
		private UIElement area;
		private UIImage barFrame;
		private Color gradientA;
		private Color gradientB;

		private int barPosX = 0;
		private int barPosY = 0;

		public override void OnInitialize()
		{
			// Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
			// UIElement is invisible and has no padding. You can use a UIPanel if you wish for a background.
			area = new UIElement();
			area.Left.Set((Main.screenWidth / 5) * 2 - (area.Width.Pixels / 2), 0f); // Place the resource bar to the left of the hearts.
			//area.Top.Set((Main.screenHeight / 2) - (barFrame.Height.Pixels) + 80, 0f); // Placing it just a bit below the top of the screen.
			area.Width.Set(138, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
			area.Height.Set(44, 0f);
			area.Top.Set(80, 0f);

			//barFrame = new UIImage(Request<Texture2D>("excels/UI/RadianceBarFrame"));
			//barFrame.Left.Set(22, 0f);
			//barFrame.Top.Set(0, 0f);
			//barFrame.Width.Set(138, 0f);
			//barFrame.Height.Set(44, 0f);

	//		text = new UIText("0/0", 0.8f); // text to show stat
	//		text.Width.Set(138, 0f);
	//		text.Height.Set(34, 0f);
	//		text.Top.Set(40, 0f);
	//		text.Left.Set(0, 0f);

			gradientA = new Color(255, 189, 0); // darker
			gradientB = new Color(255, 242, 0); // lighter

		//	area.Append(text);
			//area.Append(barFrame);
			Append(area);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			// This prevents drawing unless we are using an ExampleDamageItem
			//if (!(Main.LocalPlayer.HeldItem.ModItem is ClericDamageItem))
			//	return;

			base.Draw(spriteBatch);
			//area.

			// draw the text when hovered over
			float posX = Main.MouseWorld.X - Main.screenPosition.X;
			float posY = Main.MouseWorld.Y - Main.screenPosition.Y;

			Vector2 drawPos = (Main.MouseWorld - Main.screenPosition);
			Rectangle hitbox = area.GetInnerDimensions().ToRectangle();
			var modPlayer = Main.LocalPlayer.GetModPlayer<ClericClassPlayer>();

			drawPos.X += 20;
			drawPos.Y += 8;
			//int widthMod = 12;
			//int heightMod = 8;
			if (posX > hitbox.X && posX < hitbox.X + hitbox.Width && posY > hitbox.Y && posY < hitbox.Y + hitbox.Height)
				spriteBatch.DrawString(FontAssets.MouseText.Value, $"{modPlayer.radianceStatCurrent} / {modPlayer.radianceStatMax2}", drawPos, Color.White);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			var modPlayer = Main.LocalPlayer.GetModPlayer<ClericClassPlayer>();
			// Calculate quotient
			float quotient = (float)modPlayer.radianceStatCurrent / modPlayer.radianceStatMax2; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
			quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

			// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
			Rectangle hitbox = area.GetInnerDimensions().ToRectangle();
			hitbox.X += 10; // 12
			hitbox.Width -= 26; // 24
			hitbox.Y += 4; // 8
			hitbox.Height -= 8; // 16

			// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);

			// draw a background to the container
			for (int i = 0; i < right - left; i++)
            {
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Black);
			}

			Texture2D inside = ModContent.Request<Texture2D>("excels/UI/RadianceBarInside").Value;
			Color grad1 = new Color(255, 187, 140, 100);
			if (Main.LocalPlayer.GetModPlayer<SnowFlowerPlayer>().SnowFlowerConsumed)
			{
				inside = ModContent.Request<Texture2D>("excels/UI/RadianceBarInsideSnow").Value;
				grad1 = new Color(142, 183, 242, 100);
			}

			if (Main.LocalPlayer.GetModPlayer<excelPlayer>().hyperionHeart)
			{
				inside = ModContent.Request<Texture2D>("excels/UI/RadianceBarInsideHyper").Value;
				grad1 = new Color(130, 145, 238, 100);
			}
			if (Main.LocalPlayer.HasBuff(ModContent.BuffType<Buffs.ClericCld.AnguishedSoul>()))
            {
				inside = ModContent.Request<Texture2D>("excels/UI/RadianceBarInsideAnguish").Value;
				grad1 = new Color(216, 34, 106, 100);
			}

			// draws the current amount
			for (int i = 0; i < steps; i += 1)
			{
				//float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right - left);
				spriteBatch.Draw(inside, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(grad1, new Color(255, 255, 255, 100), percent)); // Color.Lerp(c1, c2, percent));
			}

			Texture2D texture = ModContent.Request<Texture2D>("excels/UI/RadianceBarFrame").Value;
			int frameHeight = 44;
			var frame = texture.Frame(1, 2, 0, (Main.LocalPlayer.GetModPlayer<SnowFlowerPlayer>().SnowFlowerConsumed)?1:0);
			Main.EntitySpriteDraw(texture,
				new Vector2(hitbox.X+(hitbox.Width/2), hitbox.Y+(hitbox.Height/2)),
				frame, Color.White, 0, frame.Size() / 2, 1, SpriteEffects.None, 0);

		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			//if (!(Main.LocalPlayer.HeldItem.ModItem is ClericDamageItem))
			//	return;

			//if (ContainsPoint(Main.MouseScreen))
			//	Main.LocalPlayer.mouseInterface = true;

			area.Left.Set((Main.screenWidth / 9) * 3 - (area.Width.Pixels / 2)+20, 0f);
			area.Top.Set(25 , 0f);

			//area.Left.Set(((Main.screenWidth / 2) - (area.Width.Pixels / 2)), 0);
			//area.Top.Set(((Main.screenHeight / 2) - (barFrame.Height.Pixels) + 80), 0);
			//barPosX = (int)(((Main.screenPosition.X / 2) - (barFrame.Width.Pixels / 2)) * Main.UIScale);
			//barPosY = (int)(((Main.screenPosition.Y / 2) - (barFrame.Height.Pixels / 2) + 40) * Main.UIScale);

			var modPlayer = Main.LocalPlayer.GetModPlayer<ClericClassPlayer>();
			// Setting the text per tick to update and show our resource values.
		}
	}
}
