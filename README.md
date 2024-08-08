# Bimil Engine

<img src="https://i.imgur.com/yeNco0X.png" width="580" title="The engine in action">

### TODO:

#### Refactoring

- [ ] Prefer classes over tuples (e.g. with animation's (TimeSpan, Texture) collection).
- [ ] Enrich the documentation.

#### Bug fixing

- [ ] The grid cell size calculating is not working correctly when the game is scaled on the fullscreen.
- [ ] The RectangleDrawShape's corner rounding (in SpriteBatchExtensions.DrawRectangle) is done partially, finish it.
- [ ] Create color fill logic for draws.

#### New features

- [x] Create better architecture (Bimil.Engine and Bimil.Game as own projects).
- [x] Create better scene creation logic.
- [ ] Create GUI logic.